using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer.Implementations.Gestures
{
    public class GestureSwipeLeftToRight : Gesture
    {
        private const double HandToLeftThreshold = 0.2;

        protected override bool TestInitialConditions(Body body)
        {
            Joint leftHand = body.Joints[JointType.HandLeft];
            Joint spine = body.Joints[JointType.SpineMid];

            return leftHand.Position.X > spine.Position.X + HandToLeftThreshold;
        }

        protected override bool TestPose(Body body)
        {
            Joint leftHand = body.Joints[JointType.HandLeft];
            Joint leftShoulder = body.Joints[JointType.ShoulderLeft];

            return leftHand.Position.Y > leftShoulder.Position.Y;
        }

        protected override bool TestDynamicPoses(Body body)
        {
            Joint leftHand = body.Joints[JointType.HandLeft];
            Joint previousLeftHand = GetPreviousJointState(body, JointType.HandLeft);

            if (previousLeftHand != null)
            {
                return leftHand.Position.X > previousLeftHand.Position.X;
            }

            return false;
        }

        protected override bool TestEndingConditions(Body body)=> true;

        private Joint GetPreviousJointState(Body body, JointType jointType)
        {
            return body.Joints[jointType];
        }
    }
}
