using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer
{
    public abstract class Gesture : BaseGesture
    {
        private bool IsRecognitionRunning;
        private int MinNbOfFrames;
        private int MaxNbOfFrames;
        private int mCurrentFrameCount;

        protected abstract bool TestInitialConditions(Body body);
        protected abstract bool TestPose(Body body);
        protected abstract bool TestDynamicPoses(Body body);
        protected abstract bool TestEndingConditions(Body body);

        public override void TestGesture(Body body)
        {
            if (!IsRecognitionRunning && TestInitialConditions(body))
            {
                IsRecognitionRunning = true;
            }

            if (IsRecognitionRunning)
            {
                mCurrentFrameCount++;

                if (TestPose(body) && TestRunningGesture(body) && TestEndingConditions(body))
                {
                    OnGestureRecognized(body);
                    IsRecognitionRunning = false;
                    mCurrentFrameCount = 0;
                }
            }
        }

        private bool TestRunningGesture(Body body)
        {
            return TestDynamicPoses(body);
        }
    }
}
