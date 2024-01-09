using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Kinect
{
    public class KinectManager
    {
        public KinectSensor KinectSensor { get; set; }
        public bool Status { get; private set; } = false;
        public string StatusText { get; private set; } = "Indisponible";

        public KinectManager()
        {
            KinectSensor = KinectSensor.GetDefault();
            KinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
        }

        public void StartSensor()
        {
            if (KinectSensor != null && !KinectSensor.IsOpen)
            {
                KinectSensor.Open();
            }
        }

        public void StopSensor()
        {
            if (KinectSensor != null && KinectSensor.IsAvailable && KinectSensor.IsOpen)
            {
                KinectSensor.Close();
            }
        }

        public void KinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs args)
        {
            if(KinectSensor != null && args.IsAvailable && KinectSensor.IsOpen)
            {
                Status = true;
                StatusText = "Kinect Lancé !";
                Debug.WriteLine("[LOG - KinectManager] (KinectSensor_IsAvailableChanged) : Kinect Start");
            }
            else
            {
                Status = false;
                StatusText = "Kinect Eteint !";
                Debug.WriteLine("[LOG - KinectManager] (KinectSensor_IsAvailableChanged) : Kinect Stop");
            }
        }


    }
}
