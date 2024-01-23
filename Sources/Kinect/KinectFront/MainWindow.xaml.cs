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
    public partial class MainWindow : Window
    {
        public KinectStream KinectStream { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //Color Stream
            /*KinectStream = new ColorStream(new KinectManager());
            KinectStream.Start();
            DataContext = this;*/

            //Depth Stream
            /*KinectStream = new DepthStream(new KinectManager());
            KinectStream.Start();
            DataContext = this;*/

            //InfraRed Stream
            KinectStream = new InfraRedStream(new KinectManager());
            KinectStream.Start();
            DataContext = this;


        }



    }
}