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
        public MainWindow()
        {
            InitializeComponent();
        }

        private double getXPoint(double xPoint)
        {
            double width = graphPlace.ActualWidth;
            double wcenter = width / 2;
            int scale = (int)graphScale.Value;
            int centerPoint = 0;

            for (int x = scale; x < width; x += scale)
                if ((x >= wcenter) && (x < wcenter + scale))
                {
                    centerPoint = x;
                }

            double newPoint = centerPoint + (xPoint * scale);

            return newPoint;
        }

        private double getYPoint(double yPoint)
        {
            double height = graphPlace.ActualHeight;
            double hcenter = height / 2;
            int scale = (int)graphScale.Value;
            int centerPoint = 0;

            for (int x = scale; x < height; x += scale)
                if ((x >= hcenter) && (x < hcenter + scale))
                {
                    centerPoint = x;
                }

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
            
            double wcenter = width / 2; 
            double hcenter = height / 2;

            graphPlace.Children.Clear();

            for (int x = scale; x < width; x += scale)
                if ((x >= wcenter) && (x < wcenter + scale))
                    drawLineAbsolute(x, 0, x, height, Brushes.SkyBlue);
                else
                    drawLineAbsolute(x, 0, x, height, Brushes.DarkGray);

            for (int y = scale; y < height; y += scale)
                if ((y > hcenter) && (y <hcenter + scale))
                    drawLineAbsolute(0, y, width, y, Brushes.SkyBlue);
                else
                    drawLineAbsolute(0, y, width, y, Brushes.DarkGray);
        }

        private void graphPlace_Loaded(object sender, RoutedEventArgs e)
        {
            graphPlaceBackground((int)graphScale.Value);
        }

        private void graphPlace_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            graphPlaceBackground((int)graphScale.Value);
        }

        private void graphScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            graphPlaceBackground((int)graphScale.Value);
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            drawLine(0, 0, 5, 5, Brushes.Yellow);
            drawLine(0, 0, -5, 5, Brushes.Red, 2);
            drawLine(0, 0, -5, -5, Brushes.Green, 3);
            drawLine(0, 0, 5, -5, Brushes.Blue, 4);
        }
    }
}
