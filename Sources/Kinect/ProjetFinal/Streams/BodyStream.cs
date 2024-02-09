using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;

namespace Model.Kinect.Streams
{
    public class BodyStream : KinectStream
    {
        public List<Tuple<JointType, JointType>> Bones { get; private set; }

        #region Fields
        private BodyFrameReader _bodyFrameReader = null;
        private FrameDescription _depthFrameDescription = null;
        private CoordinateMapper _coordinateMapper = null;
        #endregion

        public BodyStream(KinectManager mgr)
        {
            KinectManager = mgr;

            Bones = new List<Tuple<JointType, JointType>>();

            Bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            Bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            Bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            Bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            Bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            Bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            Bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            Bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            // Right Arm
            Bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            Bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            Bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            Bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            Bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            // Left Arm
            Bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            Bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            Bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            Bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            Bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            // Right Leg
            Bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            Bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            Bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            // Left Leg
            Bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            Bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            Bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));
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

        private Point GetDepthSpacePoint(Joint joint)
        {
            CameraSpacePoint position = joint.Position;
            DepthSpacePoint depthSpacePoint = _coordinateMapper.MapCameraPointToDepthSpace(position);
            if(depthSpacePoint.X == float.NegativeInfinity || depthSpacePoint.Y == float.NegativeInfinity) return new Point(0.1, 0.1);
            return new Point(depthSpacePoint.X, depthSpacePoint.Y);
        }

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
                    Canva.Children.Clear();

                    foreach (Body body in bodies)
                    {
                        if (body.IsTracked)
                        {
                            foreach (Joint joint in body.Joints.Values)
                            {
                                CameraSpacePoint position = joint.Position;
                                DepthSpacePoint depthSpacePoint = _coordinateMapper.MapCameraPointToDepthSpace(position);
                                if (depthSpacePoint.X == float.NegativeInfinity || depthSpacePoint.Y == float.NegativeInfinity) break;


                                Ellipse ellipse = new Ellipse
                                {
                                    Fill = Brushes.Red,
                                    Width = 15,
                                    Height = 15
                                };

                                Canvas.SetLeft(ellipse, depthSpacePoint.X - ellipse.Width / 2);
                                Canvas.SetTop(ellipse, depthSpacePoint.Y - ellipse.Width / 2);

                                Canva.Children.Add(ellipse);

                                foreach (Joint otherJoint in body.Joints.Values)
                                {
                                    
                                    if (Bones.Contains(new Tuple<JointType, JointType>(joint.JointType, otherJoint.JointType)))
                                    {
                                        Line line = new Line
                                        {
                                            Stroke = Brushes.Blue,
                                            X1 = depthSpacePoint.X,
                                            Y1 = depthSpacePoint.Y,
                                            X2 = GetDepthSpacePoint(otherJoint).X,
                                            Y2 = GetDepthSpacePoint(otherJoint).Y
                                        };

                                        Canva.Children.Add(line);
                                    }
                                }



                            }


                        }
                    }
                }
            }
        }





    }
}
