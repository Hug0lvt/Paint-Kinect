using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Kinect.Streams
{
    public class DepthStream : KinectStream
    {
        public DepthStream(KinectManager mgr)
        {
            Mgr = mgr;
        }

        public KinectManager Mgr { get; }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
