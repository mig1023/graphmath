using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace graph.math
{
    public partial class MainWindow : Window
    {
        Point p;
        bool moveGraphPlace = false;
        bool graphDrawAlready = false;
        double moveX = 0;
        double moveY = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private double getXPoint(double xPoint)
        {
            double width = graphPlace.ActualWidth;
            double wCenter = moveX + (width / 2);
            int scale = (int)graphScale.Value;
            double xMargin = wCenter % scale;
            double centerPoint = 0;

            for (double x = xMargin; x < width; x += scale)
                if ((x >= wCenter) && (x < wCenter + scale))
                    centerPoint = x;

            double newPoint = centerPoint + (xPoint * scale);

            return newPoint;
        }

        private double getYPoint(double yPoint)
        {
            double height = graphPlace.ActualHeight;
            double hCenter = moveY + (height / 2);
            int scale = (int)graphScale.Value;
            double yMargin = hCenter % scale;
            double centerPoint = 0;

            for (double x = yMargin; x < height; x += scale)
                if ((x >= hCenter) && (x < hCenter + scale))
                    centerPoint = x;

            double newPoint = centerPoint - (yPoint * scale);

            return newPoint;
        }

        private void drawLine(double x1, double y1, double x2, double y2, Brush color, int width = 1)
        {
            drawLineAbsolute(
                x1: getXPoint(x1),
                y1: getYPoint(y1),
                x2: getXPoint(x2),
                y2: getYPoint(y2),
                color: color,
                width: width
           );
        }

        private void drawLineAbsolute(double x1, double y1, double x2, double y2, Brush color, int width = 1)
        {
            var line = new Line()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = color,
                StrokeThickness = width,
                SnapsToDevicePixels = true
            };
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            graphPlace.Children.Add(line);
        }

        void graphPlaceBackground(int scale)
        {
            var width = graphPlace.ActualWidth;
            var height = graphPlace.ActualHeight;
            
            double wCenter = moveX + ( width / 2 ); 
            double hCenter = moveY + ( height / 2 );

            double xMargin = wCenter % scale;
            double yMargin = hCenter % scale;

            graphPlace.Children.Clear();

            for (double x = xMargin; x < width; x += scale)
                if ((x >= wCenter) && (x < wCenter + scale))
                    drawLineAbsolute(x, 0, x, height, Brushes.SkyBlue);
                else
                    drawLineAbsolute(x, 0, x, height, Brushes.DimGray);

            for (double y = yMargin; y < height; y += scale)
                if ((y >= hCenter) && (y < hCenter + scale))
                    drawLineAbsolute(0, y, width, y, Brushes.SkyBlue);
                else
                    drawLineAbsolute(0, y, width, y, Brushes.DimGray);
        }

        private void graphPlace_Loaded(object sender, RoutedEventArgs e)
        {
            graphPlaceBackground((int)graphScale.Value);
        }

        private void graphPlace_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            graphPlaceBackground((int)graphScale.Value);
            if (graphDrawAlready) primitiveExample();
        }

        private void graphScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            graphPlaceBackground((int)graphScale.Value);
            if (graphDrawAlready) primitiveExample();
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            primitiveExample();
            graphDrawAlready = true;
        }

        private void graphPlace_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Control c = sender as Control;
            Mouse.Capture(c);
            p = Mouse.GetPosition(c);
            moveGraphPlace = true;
        }

        private void graphPlace_MouseMove(object sender, MouseEventArgs e)
        {
            if (moveGraphPlace)
            {
                Control c = sender as Control;

                moveX = e.GetPosition(null).X - p.X;
                moveY = e.GetPosition(null).Y - p.Y;

                graphPlaceBackground((int)graphScale.Value);
                if (graphDrawAlready) primitiveExample();
            }
        }

        private void graphPlace_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            moveGraphPlace = false;
        }

        private void graphPlace_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.Capture(null);
            moveGraphPlace = false;
        }

        private void primitiveExample()
        {
            drawLine(0, 0, 5, 5, Brushes.Yellow);
            drawLine(0, 0, -5, 5, Brushes.Red, 2);
            drawLine(0, 0, -5, -5, Brushes.Green, 3);
            drawLine(0, 0, 5, -5, Brushes.Blue, 4);
        }
    }
}
