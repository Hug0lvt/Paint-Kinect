using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer.Implementations.Postures
{
    public class PostureHandsForward : Posture
    {
        private const double HandsForwardThreshold = 0.2;

        protected override bool TestPosture(Body body)
        {
            Joint leftHand = body.Joints[JointType.HandLeft];
            Joint rightHand = body.Joints[JointType.HandRight];
            Joint spine = body.Joints[JointType.SpineMid];

            bool leftHandForward = leftHand.Position.Z < spine.Position.Z - HandsForwardThreshold;
            bool rightHandForward = rightHand.Position.Z < spine.Position.Z - HandsForwardThreshold;

            return leftHandForward && rightHandForward;
        }
    }
}
