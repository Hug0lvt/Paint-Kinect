using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model.Kinect
{
    public class KinectStreamsFactory
    {
        Dictionary<KinectStreams, Func<KinectStream>> streamFactory;
        KinectStream currentStream;
        
         public KinectStreamsFactory(KinectManager mgr) {
        }
    }
}
