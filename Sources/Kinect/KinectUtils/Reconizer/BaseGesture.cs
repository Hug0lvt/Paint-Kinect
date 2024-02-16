using KinectUtils.Events;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer
{
    public abstract class BaseGesture
    {
        public event EventHandler<GestureRecognizedEventArgs> GestureRecognized;

        protected void OnGestureRecognized(Body body)
        {
            GestureRecognized?.Invoke(this, new GestureRecognizedEventArgs(body, GestureName));
        }

        public string GestureName { get; protected set; }

        public abstract void TestGesture(Body body);
    }
}
