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

        public KinectStream KinectStreamPaint 
        {
            get => _kinectStreamPaint; 
            set
            {
                _kinectStreamPaint = value;
                OnPropertyChanged();
            }
        }
        private KinectStream _kinectStreamPaint;

        public KinectStream KinectStreamTrackedHand
        {
            get => _kinectStreamTrackedHand;
            set
            {
                _kinectStreamTrackedHand = value;
                OnPropertyChanged();
            }
        }
        private KinectStream _kinectStreamTrackedHand;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //Init default Stream
            KinectStreamPaint = new HandStream(new KinectManager());
            KinectStreamPaint.Start();

            KinectStreamTrackedHand = new HandTrackedStream(new KinectManager());
            KinectStreamTrackedHand.Start();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (KinectStreamPaint == null) return;
            if (KinectStreamPaint.KinectManager == null) return;
            if (KinectStreamPaint.KinectManager.Status) KinectStreamPaint.Stop();

            if (KinectStreamTrackedHand == null) return;
            if (KinectStreamTrackedHand.KinectManager == null) return;
            if (KinectStreamTrackedHand.KinectManager.Status) KinectStreamTrackedHand.Stop();
        }

        
    }
}