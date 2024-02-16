using Model.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectFront.ViewModel
{
    public class KinectManagerViewModel : INotifyPropertyChanged
    {
        private KinectManager _kinectManager;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public KinectManager KinectManager
        {
            get => _kinectManager;
            set
            {
                if (_kinectManager != value)
                {
                    _kinectManager = value;
                    OnPropertyChanged(nameof(KinectManager));
                }
            }
        }

        public KinectManagerViewModel()
        {
            KinectManager = new KinectManager();
        }

    }
}
