using Microsoft.Kinect;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Model.Kinect.Streams
{
    public class BodyStream : KinectStream
    {

        #region Fields
        private BodyFrameReader _bodyFrameReader = null;
        #endregion

        public BodyStream(KinectManager mgr)
        {
            KinectManager = mgr;
        }

        #region Start and Stop
        public override void Start()
        {
            KinectManager.StartSensor();
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
            //Canva.Children.Clear();
            Canva.Background = new SolidColorBrush(Colors.Green);

            /*using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    Body[] bodies = new Body[bodyFrame.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    Canva.Children.Clear();
                    Canva.Background = new SolidColorBrush(Colors.Green);

                    foreach (Body body in bodies)
                    {
                        if (body.IsTracked)
                        {
                            foreach (Joint joint in body.Joints.Values)
                            {
                                DepthSpacePoint depthSpacePoint = KinectManager.KinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(joint.Position);
                                Point point = new Point(depthSpacePoint.X, depthSpacePoint.Y);

                                Ellipse ellipse = new Ellipse
                                {
                                    Width = 20,
                                    Height = 20,
                                    Fill = Brushes.Red
                                };
                                Canva.SetLeft(ellipse, point.X - ellipse.Width / 2);
                                Canva.SetTop(ellipse, point.Y - ellipse.Height / 2);

                                Canva.Children.Add(ellipse);
                            }
                        }
                    }

                }
            }*/
        }





    }
}
