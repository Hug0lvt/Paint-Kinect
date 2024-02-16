using KinectFront.Streams;
using KinectFront.ViewModel;
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
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
        }

        private void ColorStream_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.KinectStream = new ColorStream(new KinectManager());
            _viewModel.KinectStream.Start();
        }

        private void InfraRedStream_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.KinectStream = new InfraRedStream(new KinectManager());
            _viewModel.KinectStream.Start();
        }

        private void DepthStream_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.KinectStream = new DepthStream(new KinectManager());
            _viewModel.KinectStream.Start();
        }

        private void BodyStream_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.KinectStream = new BodyStream(new KinectManager());
            _viewModel.KinectStream.Start();
        }

        private void BodyAndColorStream_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.KinectStream = new BodyAndColorStream(new KinectManager());
            _viewModel.KinectStream.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_viewModel.KinectStream == null) return;
            if (_viewModel.KinectStream.KinectManager == null) return;
            if (_viewModel.KinectStream.KinectManager.Status) _viewModel.KinectStream.Stop();
        }

        
    }
}