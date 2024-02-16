using KinectUtils.Events;
using KinectUtils.Reconizer.Implementations.Postures;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;

namespace Model.Kinect.Streams
{
    public class HandStream : KinectStream
    {

        #region Fields
        private BodyFrameReader _bodyFrameReader = null;
        private FrameDescription _depthFrameDescription = null;
        private CoordinateMapper _coordinateMapper = null;
        private SolidColorBrush _colorBrush = Brushes.Red;
        #endregion

        #region Postures
        PostureRightHandOpen _postureRightHandOpen = new PostureRightHandOpen();
        PostureLeftHandOpen _postureLeftHandOpen = new PostureLeftHandOpen();
        PostureLeftHandClosed _postureLeftHandClosed =new PostureLeftHandClosed();
        #endregion

        public HandStream(KinectManager mgr)
        {
            KinectManager = mgr;
        }

        #region Start and Stop
        public override void Start()
        {
            KinectManager.StartSensor();
            _depthFrameDescription = KinectManager.KinectSensor.DepthFrameSource.FrameDescription;
            _coordinateMapper = KinectManager.KinectSensor.CoordinateMapper;
            _bodyFrameReader = KinectManager.KinectSensor.BodyFrameSource.OpenReader();
            _bodyFrameReader.FrameArrived += KinectSensor_BodyFrameArrived;
            Canva = new Canvas();
            KinectManager.KinectSensor.Open();
            Width = _depthFrameDescription.Width;
            Height = _depthFrameDescription.Height;

            //Events
            _postureRightHandOpen.GestureRecognized += OnPostureRightHandOpenRecognized;
            _postureLeftHandOpen.GestureRecognized += OnPostureLeftHandOpenRecognized;
            _postureLeftHandClosed.GestureRecognized += OnPostureLeftHandClosedRecognized;
        }

        public override void Stop()
        {
            //Events
            _postureRightHandOpen.GestureRecognized -= OnPostureRightHandOpenRecognized;
            _postureLeftHandOpen.GestureRecognized -= OnPostureLeftHandOpenRecognized;
            _postureLeftHandClosed.GestureRecognized -= OnPostureLeftHandClosedRecognized;

            _bodyFrameReader.Dispose();
            KinectManager.StopSensor();
        }
        #endregion

        private void KinectSensor_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            Canva.Width = _depthFrameDescription.Width;
            Canva.Height = _depthFrameDescription.Height;
            Joint centerJoint;

            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    Body[] bodies = new Body[bodyFrame.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    //Canva.Children.Clear();
                }
            }
        }

        private void OnPostureRightHandOpenRecognized(object sender, GestureRecognizedEventArgs e)
        {
            if(sender is Body body && body.IsTracked)
            {
                Joint joint = body.Joints[JointType.HandLeft];
                CameraSpacePoint position = joint.Position;
                DepthSpacePoint depthSpacePoint = _coordinateMapper.MapCameraPointToDepthSpace(position);
                if (depthSpacePoint.X != float.NegativeInfinity && depthSpacePoint.Y != float.NegativeInfinity)
                {
                    Ellipse ellipse = new Ellipse
                    {
                        Width = 10,
                        Height = 10
                    };
                    ellipse.Fill = _colorBrush;

                    Canvas.SetLeft(ellipse, depthSpacePoint.X - ellipse.Width / 2);
                    Canvas.SetTop(ellipse, depthSpacePoint.Y - ellipse.Width / 2);

                    Canva.Children.Add(ellipse);
                }
            }   
        }

        private void OnPostureLeftHandOpenRecognized(object sender, GestureRecognizedEventArgs e)
        {
            if (sender is Body body && body.IsTracked)
            {
                Joint joint = body.Joints[JointType.HandLeft];
                CameraSpacePoint position = joint.Position;
                DepthSpacePoint depthSpacePoint = _coordinateMapper.MapCameraPointToDepthSpace(position);
                if (depthSpacePoint.X != float.NegativeInfinity && depthSpacePoint.Y != float.NegativeInfinity)
                {
                    Joint handLeftJoint = body.Joints[JointType.HandLeft];
                    float handLeftX = handLeftJoint.Position.X;
                    float handLeftY = handLeftJoint.Position.Y;

                    byte interpolatedR = (byte)(handLeftY * 255);
                    byte interpolatedG = (byte)(handLeftX * 255);
                    byte interpolatedB = (byte)((1 - handLeftX) * 255);

                    _colorBrush = new SolidColorBrush(Color.FromRgb(interpolatedR, interpolatedG, interpolatedB));
                }
            }
        }

        private void OnPostureLeftHandClosedRecognized(object sender, GestureRecognizedEventArgs e)
        {
            if (sender is Body body && body.IsTracked)
            {
                Joint joint = body.Joints[JointType.HandLeft];
                CameraSpacePoint position = joint.Position;
                DepthSpacePoint depthSpacePoint = _coordinateMapper.MapCameraPointToDepthSpace(position);
                if (depthSpacePoint.X != float.NegativeInfinity && depthSpacePoint.Y != float.NegativeInfinity)
                {

                    
                    Ellipse ellipse = new Ellipse
                    {
                        Width = 20,
                        Height = 20
                    };
                    ellipse.Fill = Brushes.White;

                    Canvas.SetLeft(ellipse, depthSpacePoint.X - ellipse.Width / 2);
                    Canvas.SetTop(ellipse, depthSpacePoint.Y - ellipse.Width / 2);

                    Canva.Children.Add(ellipse);
                }
            }
        }

    }
}
