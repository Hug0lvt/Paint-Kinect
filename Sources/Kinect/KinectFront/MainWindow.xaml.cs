using Microsoft.Kinect;
using Model.Kinect;
using Model.Kinect.Streams;
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

namespace KinectFront
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
            KinectStream = new ColorStream(new KinectManager());
            KinectStream.Start();


            //Body Stream
            /*KinectStream = new BodyStream(new KinectManager());
            KinectStream.Start();
            DataContext = this;*/


        }

        private void ColorStream_Click(object sender, RoutedEventArgs e)
        {
            KinectStream = new ColorStream(new KinectManager());
            KinectStream.Start();
        }

        private void InfraRedStream_Click(object sender, RoutedEventArgs e)
        {
            KinectStream = new InfraRedStream(new KinectManager());
            KinectStream.Start();
        }

        private void DepthStream_Click(object sender, RoutedEventArgs e)
        {
            KinectStream = new DepthStream(new KinectManager());
            KinectStream.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (KinectStream.KinectManager.Status) KinectStream.Stop();
        }
    }
}