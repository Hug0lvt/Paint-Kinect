using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Kinect
{
    public class KinectManager
    {
        public KinectSensor KinectSensor { get; set; }
        public bool Status { get; private set; }
        public string StatusText { get; private set; }

        public void StartSensor()
        {

        }

        public void StopSensor()
        {

        }

        //public void KinectSensor_IsAvailableChanged(sender, args)


    }
}
