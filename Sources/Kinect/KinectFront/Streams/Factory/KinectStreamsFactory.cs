using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Model.Kinect.Streams.Enum;

namespace Model.Kinect.Streams.Factory
{
    public class KinectStreamsFactory
    {
        public Dictionary<KinectStreams, Func<KinectStream>> streamFactory { get; private set; }
        public KinectStream currentStream { get; private set; }

        public KinectStreamsFactory(KinectManager mgr)
        {
            streamFactory = new Dictionary<KinectStreams, Func<KinectStream>>();
            streamFactory.Add(KinectStreams.None, null);
            streamFactory.Add(KinectStreams.Color, () => { return new ColorStream(mgr); });
            streamFactory.Add(KinectStreams.Infrared, () => { return new InfraRedStream(mgr); });
            streamFactory.Add(KinectStreams.Depth, () => { return new DepthStream(mgr); });
            streamFactory.Add(KinectStreams.Body, () => { return new BodyStream(mgr); });

        }
    }
}
