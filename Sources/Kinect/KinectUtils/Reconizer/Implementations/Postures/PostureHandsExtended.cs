using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer.Implementations.Postures
{
    public class PostureHandsExtended : Posture
    {
        private const double HandsExtendedThreshold = 0.2;

        protected override bool TestPosture(Body body)
        {
            Joint leftHand = body.Joints[JointType.HandLeft];
            Joint rightHand = body.Joints[JointType.HandRight];
            Joint spine = body.Joints[JointType.SpineMid];

            bool leftHandExtended = Math.Abs(leftHand.Position.X - spine.Position.X) > HandsExtendedThreshold;
            bool rightHandExtended = Math.Abs(rightHand.Position.X - spine.Position.X) > HandsExtendedThreshold;

            return leftHandExtended && rightHandExtended;
        }
    }
}
