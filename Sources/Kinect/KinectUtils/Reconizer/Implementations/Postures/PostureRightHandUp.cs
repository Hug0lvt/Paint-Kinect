using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer.Implementations.Postures
{
    public class PostureRightHandUp : Posture
    {
        protected override bool TestPosture(Body body)
        {
            Joint rightHand = body.Joints[JointType.HandRight];
            Joint shoulder = body.Joints[JointType.ShoulderRight];

            return rightHand.Position.Y > shoulder.Position.Y;
        }
    }
}
