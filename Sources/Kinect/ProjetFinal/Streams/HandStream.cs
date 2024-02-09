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
    public class HandStream : KinectStream
    {
        public List<Tuple<JointType, JointType>> Bones { get; private set; }

        #region Fields
        private BodyFrameReader _bodyFrameReader = null;
        private FrameDescription _depthFrameDescription = null;
        private CoordinateMapper _coordinateMapper = null;
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
            Joint centerJoint;

            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    Body[] bodies = new Body[bodyFrame.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    //Canva.Children.Clear();

                    foreach (Body body in bodies)
                    {
                        if (body.IsTracked)
                        {
                            // Afficher seulement les points des mains gauche et droite
                            DrawHandJoint(body, JointType.HandLeft);
                            DrawHandJoint(body, JointType.HandRight);
                        }
                    }
                }
            }
        }

        private void DrawHandJoint(Body body, JointType jointType)
        {
            Joint joint = body.Joints[jointType];
            CameraSpacePoint position = joint.Position;
            DepthSpacePoint depthSpacePoint = _coordinateMapper.MapCameraPointToDepthSpace(position);

            if (depthSpacePoint.X != float.NegativeInfinity && depthSpacePoint.Y != float.NegativeInfinity)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = 10,
                    Height = 10
                };

                if (jointType == JointType.HandLeft && body.HandLeftState == HandState.Open)
                {
                    ellipse.Fill = Brushes.Black; // Main gauche open en noir
                    ellipse.Width = 10;
                    ellipse.Height = 10;
                }
                else if (jointType == JointType.HandRight && body.HandRightState == HandState.Open)
                {
                    ellipse.Fill = Brushes.Red; // Main droite open
                    ellipse.Width = 10;
                    ellipse.Height = 10;
                }
                else if(jointType == JointType.HandRight && body.HandRightState == HandState.Closed)
                {
                    ellipse.Fill = Brushes.White; // Main droite fermé
                    ellipse.Width = 20;
                    ellipse.Height = 20;
                }

                Canvas.SetLeft(ellipse, depthSpacePoint.X - ellipse.Width / 2);
                Canvas.SetTop(ellipse, depthSpacePoint.Y - ellipse.Width / 2);

                Canva.Children.Add(ellipse);
            }
        }
    }
}
