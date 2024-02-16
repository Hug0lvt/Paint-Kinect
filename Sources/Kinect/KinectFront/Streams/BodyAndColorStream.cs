using Microsoft.Kinect;
using Model.Kinect;
using Model.Kinect.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectFront.Streams
{
    public class BodyAndColorStream : KinectStream
    {

        private ColorStream _color;
        private BodyStream _body;
        protected FrameDescription _colorFrameDescription = null;


        public BodyAndColorStream(KinectManager mgr)
        {
            KinectManager = mgr;
            _color = new ColorStream(KinectManager);
            _body = new BodyStream(KinectManager);
        }

        public override void Start()
        {
            _color.Start();
            _body.Start();
            Canva = _body.Canva;
            Bitmap = _color.Bitmap;
            _colorFrameDescription = KinectManager.KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            Width = _colorFrameDescription.Width;
            Height = _colorFrameDescription.Height;
        }

        public override void Stop()
        {
            _body.Stop();
            _color.Stop();
        }
    }
}
