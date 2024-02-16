using Microsoft.Kinect;
using Model.Kinect;
using Model.Kinect.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace KinectFront.Streams
{
    public class BodyAndColorStreamv2 : KinectStream
    {
        #region Fields
        protected FrameDescription _colorFrameDescription = null;
        private FrameDescription _depthFrameDescription = null;
        protected MultiSourceFrameReader _multiFrameSourceReader = null;
        private CoordinateMapper _coordinateMapper = null;
        private DepthSpacePoint[] _colorMappedToDepthPoints = null;
        #endregion

        public BodyAndColorStreamv2(KinectManager mgr)
        {
            KinectManager = mgr;
        }

        public override void Start()
        {
            KinectManager.StartSensor();
            _coordinateMapper = KinectManager.KinectSensor.CoordinateMapper;
            _colorFrameDescription = KinectManager.KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            _depthFrameDescription = KinectManager.KinectSensor.DepthFrameSource.FrameDescription;
            _multiFrameSourceReader = KinectManager.KinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color | FrameSourceTypes.BodyIndex);
            _multiFrameSourceReader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            Width = _colorFrameDescription.Width;
            Height = _colorFrameDescription.Height;
            _colorMappedToDepthPoints = new DepthSpacePoint[Width * Height];
            Bitmap = new WriteableBitmap(Width, Height, 96.0, 96.0, PixelFormats.Bgra32, null);
            Canva = new Canvas();
        }

        public override void Stop()
        {
            _multiFrameSourceReader.Dispose();
            KinectManager.StopSensor();
        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            int depthWidth = 0;
            int depthHeight = 0;

            DepthFrame depthFrame = null;
            ColorFrame colorFrame = null;
            BodyIndexFrame bodyIndexFrame = null;
            bool isBitmapLocked = false;

            MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();

            if (multiSourceFrame == null) return;

            try
            {
                depthFrame = multiSourceFrame.DepthFrameReference.AcquireFrame();
                colorFrame = multiSourceFrame.ColorFrameReference.AcquireFrame();
                bodyIndexFrame = multiSourceFrame.BodyIndexFrameReference.AcquireFrame();

                if ((depthFrame == null) || (colorFrame == null) || (bodyIndexFrame == null)) return;

                #region Depth Process
                FrameDescription depthFrameDescription = depthFrame.FrameDescription;

                depthWidth = depthFrameDescription.Width;
                depthHeight = depthFrameDescription.Height;

                using (KinectBuffer depthFrameData = depthFrame.LockImageBuffer())
                {
                    _coordinateMapper.MapColorFrameToDepthSpaceUsingIntPtr(
                        depthFrameData.UnderlyingBuffer,
                        depthFrameData.Size,
                        _colorMappedToDepthPoints);
                }

                depthFrame.Dispose();
                depthFrame = null;
                #endregion

                #region Color Process
                _bitmap.Lock();
                isBitmapLocked = true;

                colorFrame.CopyConvertedFrameDataToIntPtr(
                                _bitmap.BackBuffer,
                                (uint)(Width * Height * 4),
                                ColorImageFormat.Bgra);
 
                colorFrame.Dispose();
                colorFrame = null;
                #endregion

                // We'll access the body index data directly to avoid a copy
                using (KinectBuffer bodyIndexData = bodyIndexFrame.LockImageBuffer())
                {
                    unsafe
                    {
                        byte* bodyIndexDataPointer = (byte*)bodyIndexData.UnderlyingBuffer;

                        int colorMappedToDepthPointCount = _colorMappedToDepthPoints.Length;

                        fixed (DepthSpacePoint* colorMappedToDepthPointsPointer = _colorMappedToDepthPoints)
                        {
                            uint* bitmapPixelsPointer = (uint*)_bitmap.BackBuffer;

                            for (int colorIndex = 0; colorIndex < colorMappedToDepthPointCount; ++colorIndex)
                            {
                                float colorMappedToDepthX = colorMappedToDepthPointsPointer[colorIndex].X;
                                float colorMappedToDepthY = colorMappedToDepthPointsPointer[colorIndex].Y;

                                if (!float.IsNegativeInfinity(colorMappedToDepthX) &&
                                    !float.IsNegativeInfinity(colorMappedToDepthY))
                                {
                                    // Make sure the depth pixel maps to a valid point in color space
                                    int depthX = (int)(colorMappedToDepthX + 0.5f);
                                    int depthY = (int)(colorMappedToDepthY + 0.5f);

                                    // If the point is not valid, there is no body index there.
                                    if ((depthX >= 0) && (depthX < depthWidth) && (depthY >= 0) && (depthY < depthHeight))
                                    {
                                        int depthIndex = (depthY * depthWidth) + depthX;

                                        // If we are tracking a body for the current pixel, do not zero out the pixel
                                        if (bodyIndexDataPointer[depthIndex] != 0xff)
                                        {
                                            continue;
                                        }
                                    }
                                }

                                bitmapPixelsPointer[colorIndex] = 0;
                            }
                        }

                        _bitmap.AddDirtyRect(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));
                    }
                }
            }
            finally
            {
                if (isBitmapLocked)
                {
                    _bitmap.Unlock();
                }

                if (depthFrame != null)
                {
                    depthFrame.Dispose();
                }

                if (colorFrame != null)
                {
                    colorFrame.Dispose();
                }

                if (bodyIndexFrame != null)
                {
                    bodyIndexFrame.Dispose();
                }
            }
        }
    }
}
