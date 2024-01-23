using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Model.Kinect.Streams
{
    public class DepthStream : KinectStream
    {

        #region Fields
        private FrameDescription _depthFrameDescription = null;
        private DepthFrameReader _depthFrameReader = null;
        private byte[] _depthPixels = null;
        #endregion

        public DepthStream(KinectManager mgr)
        {
            KinectManager = mgr;
        }

        #region Start/Stop Methods
        public override void Start()
        {
            KinectManager.StartSensor();
            _depthFrameDescription = KinectManager.KinectSensor.DepthFrameSource.FrameDescription;
            _depthFrameReader = KinectManager.KinectSensor.DepthFrameSource.OpenReader();
            _depthFrameReader.FrameArrived += Reader_DepthFrameArrived;
            _bitmap = new WriteableBitmap(_depthFrameDescription.Width, _depthFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray8, null);
            _depthPixels = new byte[_depthFrameDescription.Width * _depthFrameDescription.Height];
        }

        public override void Stop()
        {
            _depthFrameReader.Dispose();
            KinectManager.StopSensor();
        }
        #endregion

        #region EventDepth
        private void Reader_DepthFrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {

            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
            {
                if (depthFrame != null)
                {
                    FrameDescription depthFrameDescription = depthFrame.FrameDescription;

                    using (Microsoft.Kinect.KinectBuffer depthBuffer = depthFrame.LockImageBuffer())
                    {
                        if (((_depthFrameDescription.Width * _depthFrameDescription.Height) == (depthBuffer.Size / _depthFrameDescription.BytesPerPixel)) &&
                            (_depthFrameDescription.Width == _bitmap.PixelWidth) && (_depthFrameDescription.Height == _bitmap.PixelHeight))
                        {
                            ProcessDepthFrameData(depthBuffer.UnderlyingBuffer, depthBuffer.Size, depthFrame.DepthMinReliableDistance, ushort.MaxValue);
                            RenderDepthPixels();

                        }
                    }
                }
            }

        }

        private unsafe void ProcessDepthFrameData(IntPtr depthFrameData, uint depthFrameDataSize, ushort minDepth, ushort maxDepth)
        {
            ushort* frameData = (ushort*)depthFrameData;
            for (int i = 0; i < (int)(depthFrameDataSize / _depthFrameDescription.BytesPerPixel); ++i)
            {
                ushort depth = frameData[i];
                _depthPixels[i] = (byte)(depth >= minDepth && depth <= maxDepth ? (depth / 31.25) : 0);
            }
        }

        private void RenderDepthPixels()
        {
            _bitmap.WritePixels(
                new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight),
                _depthPixels,
                _bitmap.PixelWidth,
                0);
        }
        #endregion

    }
}
