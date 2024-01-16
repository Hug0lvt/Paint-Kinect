using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Model.Kinect
{
    public abstract class KinectStream : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Fields
        protected FrameDescription _colorFrameDescription = null;
        protected ColorFrameReader _colorFrameReader = null;
        #endregion

        #region Property
        public KinectManager KinectManager 
        {
            get => _kinectManager; 
            protected set
            {
                if (_kinectManager != value)
                {
                    _kinectManager = value;
                    OnPropertyChanged();
                }
            }
        }
        private KinectManager _kinectManager = null;
        public ImageSource Bitmap { get => _bitmap; }
        protected WriteableBitmap _bitmap = null;
        #endregion

        public abstract void Start();
        public abstract void Stop();
        
    }
}
