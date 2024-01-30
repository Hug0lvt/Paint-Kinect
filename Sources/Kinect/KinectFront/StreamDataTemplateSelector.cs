using KinectFront.Streams;
using Model.Kinect;
using Model.Kinect.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KinectFront
{

    public class StreamDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? BitmapTemplate { get; set; }
        public DataTemplate? CanvasTemplate { get; set; }
        public DataTemplate? BitmapAndCanvasTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        => item switch {
            ColorStream => BitmapTemplate,
            DepthStream => BitmapTemplate,
            InfraRedStream => BitmapTemplate,
            BodyStream => CanvasTemplate,
            BodyAndColorStream => BitmapAndCanvasTemplate,
            _ => null
        };   
    }


}
