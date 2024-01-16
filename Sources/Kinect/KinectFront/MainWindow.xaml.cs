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

namespace KinectFront
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public KinectManager KinectManager { get;  private set; }
        FrameDescription colorFrameDescription;

        public ImageSource ImageSource
        {
            get
            {
                return bitmap;
            }
        }
        private WriteableBitmap bitmap;

        public MainWindow()
        {
            InitializeComponent();


            KinectManager = new KinectManager();
            KinectManager.StartSensor();

            colorFrameDescription = KinectManager.KinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            KinectManager.KinectSensor.ColorFrameSource.OpenReader().FrameArrived += Reader_ColorFrameArrived;

            bitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

            DataContext = this;
        }

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        bitmap.Lock();

                        if ((colorFrameDescription.Width == bitmap.PixelWidth) && (colorFrameDescription.Height == bitmap.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                bitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                        }

                        this.bitmap.Unlock();
                    }
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
