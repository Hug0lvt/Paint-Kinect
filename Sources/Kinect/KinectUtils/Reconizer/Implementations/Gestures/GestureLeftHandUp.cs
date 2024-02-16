using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer.Implementations.Gestures
{
    public class GestureLeftHandUp : Gesture
    {
        private const double HandAboveHeadThreshold = 0.1;

        protected override bool TestInitialConditions(Body body)
        {
            Joint leftHand = body.Joints[JointType.HandLeft];
            Joint head = body.Joints[JointType.Head];

            return leftHand.Position.Y > head.Position.Y + HandAboveHeadThreshold;
        }

        protected override bool TestPose(Body body) => true;


        protected override bool TestDynamicPoses(Body body) => true;

        protected override bool TestEndingConditions(Body body) => true;
    }
}
