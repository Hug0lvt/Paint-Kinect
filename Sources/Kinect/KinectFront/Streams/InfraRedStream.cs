using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Model.Kinect.Streams
{
    public class InfraRedStream : KinectStream
    {
        #region Property
        private InfraredFrameReader infraredFrameReader = null;
        private FrameDescription infraredFrameDescription = null;
        private const float InfraredSourceValueMaximum = (float)ushort.MaxValue;
        private const float InfraredSourceScale = 0.75f; 
        private const float InfraredOutputValueMinimum = 0.01f;
        private const float InfraredOutputValueMaximum = 1.0f;
        private byte[] infraredPixels = null;
        private const int BytesPerPixel = 4;
        public KinectManager Mgr { get; }
        #endregion
        public InfraRedStream(KinectManager mgr)
        {
            Mgr = mgr;
        }

        #region Start/Stop Methods

        public override void Start()
        {
            Mgr.StartSensor();
            infraredFrameDescription = Mgr.KinectSensor.InfraredFrameSource.FrameDescription;
            infraredFrameReader = Mgr.KinectSensor.InfraredFrameSource.OpenReader();
            this.infraredFrameReader.FrameArrived += Reader_InfraredFrameArrived;
            _bitmap = new WriteableBitmap(infraredFrameDescription.Width, infraredFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray32Float, null);
            Mgr.KinectSensor.Open();
        }

        public override void Stop()
        {
            _colorFrameReader.Dispose();
            Mgr.StopSensor();
        }
        #endregion

        #region EventInfraRed
        private void Reader_InfraredFrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            using (InfraredFrame infraredFrame = e.FrameReference.AcquireFrame())
            {
                if (infraredFrame != null)
                {
                    using (Microsoft.Kinect.KinectBuffer infraredBuffer = infraredFrame.LockImageBuffer())
                    {
                        if (((infraredFrameDescription.Width * infraredFrameDescription.Height) == (infraredBuffer.Size / infraredFrameDescription.BytesPerPixel)) &&
                            (infraredFrameDescription.Width == _bitmap.PixelWidth) && (infraredFrameDescription.Height == _bitmap.PixelHeight))
                        {
                            ProcessInfraredFrameData(infraredBuffer.UnderlyingBuffer, infraredBuffer.Size);
                        }
                    }
                }
            }
        }
        #endregion

        #region Processing

        private unsafe void ProcessInfraredFrameData(IntPtr infraredFrameData, uint infraredFrameDataSize)
        {
            ushort* frameData = (ushort*)infraredFrameData;
            this._bitmap.Lock();
            float* backBuffer = (float*)_bitmap.BackBuffer;
            for (int i = 0; i < (int)(infraredFrameDataSize / infraredFrameDescription.BytesPerPixel); ++i)
            {
                backBuffer[i] = Math.Min(InfraredOutputValueMaximum, (((float)frameData[i] / InfraredSourceValueMaximum * InfraredSourceScale) * (1.0f - InfraredOutputValueMinimum)) + InfraredOutputValueMinimum);
            }
            _bitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));

            _bitmap.Unlock();
        }
        #endregion
    }
}