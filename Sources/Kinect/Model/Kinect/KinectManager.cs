using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model.Kinect
{
    public class KinectManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public KinectSensor KinectSensor { get; set; }
        public bool Status 
        {
            get
            {
                return _status;
            } 
            private set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
        private bool _status = false;
        public string StatusText 
        {
            get
            {
                return _statusText;
            } 
            private set
            {
                _statusText = value;
                OnPropertyChanged();
            } 
        }
        private string _statusText = "Indisponible";

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

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}
