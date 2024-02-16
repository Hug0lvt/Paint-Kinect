using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Events
{
    public class GestureRecognizedEventArgs : EventArgs
    {
        public Body RecognizedBody { get; private set; }
        public string GestureName { get; private set; }

        public GestureRecognizedEventArgs(Body recognizedBody, string gestureName)
        {
            RecognizedBody = recognizedBody;
            GestureName = gestureName;
        }
    }
}
