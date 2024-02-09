using Microsoft.Kinect;
using Model.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model.Kinect.Streams;

namespace ProjetFinal
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public KinectStream KinectStream 
        {
            get => _kinectStream; 
            set
            {
                _kinectStream = value;
                OnPropertyChanged();
            }
        }
        private KinectStream _kinectStream;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //Init default Stream
            KinectStream = new HandStream(new KinectManager());
            KinectStream.Start();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (KinectStream == null) return;
            if (KinectStream.KinectManager == null) return;
            if (KinectStream.KinectManager.Status) KinectStream.Stop();
        }

        
    }
}