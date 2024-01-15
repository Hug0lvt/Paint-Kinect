using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Kinect
{
    abstract class KinectStream
    {
        KinectSensor Sensor { get; set; }
        KinectManager KinectManager { get; set; }
        public abstract void Start();
        public abstract void Stop();
    }
}
