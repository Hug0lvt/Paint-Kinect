using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer.Implementations.Postures
{
    public class PostureTwoHandsUp : Posture
    {
        private const double HandsAboveHeadThreshold = 0.1;

        protected override bool TestPosture(Body body)
        {
            Joint leftHand = body.Joints[JointType.HandLeft];
            Joint rightHand = body.Joints[JointType.HandRight];
            Joint head = body.Joints[JointType.Head];

            bool leftHandAboveHead = leftHand.Position.Y > head.Position.Y + HandsAboveHeadThreshold;
            bool rightHandAboveHead = rightHand.Position.Y > head.Position.Y + HandsAboveHeadThreshold;

            return leftHandAboveHead && rightHandAboveHead;
        }
    }

}
