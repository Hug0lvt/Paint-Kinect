using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer
{
    public abstract class Posture : BaseGesture
    {
        protected abstract bool TestPosture(Body body);

        public override void TestGesture(Body body)
        {
            if (TestPosture(body))
            {
                OnGestureRecognized(body);
            }
        }
    }

}
