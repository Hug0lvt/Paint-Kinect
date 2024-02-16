using Model.Kinect;
using Model.Kinect.Streams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectFront.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private KinectStream _kinectStream;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public KinectManagerViewModel KinectManagerViewModel { get; }

        public KinectStream KinectStream
        {
            get => _kinectStream;
            set
            {
                if (_kinectStream != value)
                {
                    _kinectStream = value;
                    OnPropertyChanged(nameof(KinectStream));
                }
            }
        }

        public MainWindowViewModel()
        {
            KinectManagerViewModel = new KinectManagerViewModel();
            KinectStream = new ColorStream(KinectManagerViewModel.KinectManager);
            KinectStream.Start();
        }
    }
}
