using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;

namespace Model.Kinect.Streams
{
    public class HandTrackedStream : KinectStream
    {
        private Point leftHandPosition;
        private Point rightHandPosition;

        #region Fields
        private BodyFrameReader _bodyFrameReader = null;
        private FrameDescription _depthFrameDescription = null;
        private CoordinateMapper _coordinateMapper = null;
        #endregion

        public HandTrackedStream(KinectManager mgr)
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
        }

        public override void Stop()
        {
            _bodyFrameReader.Dispose();
            KinectManager.StopSensor();
        }
        #endregion

        private void KinectSensor_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            Canva.Width = _depthFrameDescription.Width;
            Canva.Height = _depthFrameDescription.Height;

            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    Body[] bodies = new Body[bodyFrame.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(bodies);

                    foreach (Body body in bodies)
                    {
                        if (body.IsTracked)
                        {
                            UpdateHandPosition(body, JointType.HandLeft, ref leftHandPosition);
                            UpdateHandPosition(body, JointType.HandRight, ref rightHandPosition);
                        }
                    }

                    // Effacer le canevas
                    Canva.Children.Clear();

                    // Afficher les mains aux nouvelles positions
                    DrawHandPosition(leftHandPosition, Brushes.Blue);
                    DrawHandPosition(rightHandPosition, Brushes.Blue);
                }
            }
        }

        private void UpdateHandPosition(Body body, JointType jointType, ref Point handPosition)
        {
            Joint joint = body.Joints[jointType];
            CameraSpacePoint position = joint.Position;
            DepthSpacePoint depthSpacePoint = _coordinateMapper.MapCameraPointToDepthSpace(position);

            if (depthSpacePoint.X != float.NegativeInfinity && depthSpacePoint.Y != float.NegativeInfinity)
            {
                handPosition = new Point(depthSpacePoint.X, depthSpacePoint.Y);
            }
        }

        private void DrawHandPosition(Point handPosition, SolidColorBrush color)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = 5,
                Height = 5,
                Fill = color
            };

            Canvas.SetLeft(ellipse, handPosition.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, handPosition.Y - ellipse.Width / 2);

            Canva.Children.Add(ellipse);
        }
    }
}
