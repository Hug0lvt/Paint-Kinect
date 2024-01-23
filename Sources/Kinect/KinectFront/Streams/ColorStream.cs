using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Model.Kinect.Streams
{
    public class ColorStream : KinectStream
    {

        #region Fields
        protected FrameDescription _colorFrameDescription = null;
        protected ColorFrameReader _colorFrameReader = null;
        #endregion

        public ColorStream(KinectManager mgr)
        {
            KinectManager = mgr;   
        }

        #region Start/Stop Methods
        public override void Start()
        {
            KinectManager.StartSensor();
            _colorFrameDescription = KinectManager.KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            _colorFrameReader = KinectManager.KinectSensor.ColorFrameSource.OpenReader();
            _colorFrameReader.FrameArrived += Reader_ColorFrameArrived;
            Bitmap = new WriteableBitmap(_colorFrameDescription.Width, _colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

        }

        public override void Stop()
        {
            _colorFrameReader.Dispose();
            KinectManager.StopSensor();
        }
        #endregion

        #region EventColor
        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        _bitmap.Lock();
                        if ((colorFrameDescription.Width == _bitmap.PixelWidth) && (colorFrameDescription.Height == _bitmap.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                _bitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);
                            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));
                        }

                        _bitmap.Unlock();
                    }
                }
            }
        }
        #endregion
    }
}
