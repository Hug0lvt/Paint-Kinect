﻿using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils.Reconizer.Implementations.Postures
{
    public class PostureLeftHandClosed : Posture
    {
        protected override bool TestPosture(Body body)
        {
            return body.HandLeftState == HandState.Closed;
        }
    }
}
