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
        double xMove = 0;
        double yMove = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private double getPoint(double xyPoint, double actualSize, double xyMove, bool isY)
        {
            double center = xyMove + (actualSize / 2);
            int scale = (int)graphScale.Value;

            double centerPoint = 0;
            double newPoint;

            for (double x = center % scale; x < actualSize; x += scale)
                if ((x >= center) && (x < center + scale))
                    centerPoint = x;

            newPoint = centerPoint + ((xyPoint * scale) * (isY ? 1 : -1));

            return newPoint;
        }

        private double getXPoint(double xPoint)
        {
            return getPoint(
                xyPoint: xPoint,
                actualSize: graphPlace.ActualWidth,
                xyMove: xMove,
                isY: true
            );
        }

        private double getYPoint(double yPoint)
        {
            return getPoint(
                xyPoint: yPoint,
                actualSize: graphPlace.ActualHeight,
                xyMove: yMove,
                isY: false
            );
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

        string[] parseGraphText()
        {
            return graphText.Text.Split('\n');
        }

        void drawAlgorithmLine(string algorithmLine)
        {
            if (algorithmLine.IndexOf("line") > -1)
            {
                int start = algorithmLine.IndexOf('(')+1;
                int end = algorithmLine.IndexOf(')')-algorithmLine.IndexOf('(')-1;

                String paramLine = algorithmLine.Substring(start, end);

                string[] paramLines = paramLine.Split(',');

                int[] param = Array.ConvertAll(paramLines, n => int.Parse(n));

                drawLine(param[0], param[1], param[2], param[3], Brushes.White);
            }
        }

        void drawAlgorithm()
        {
            string[] algorithmLines = parseGraphText();

            foreach(string algorithmLine in algorithmLines)
                drawAlgorithmLine(algorithmLine);
        }

        void graphPlaceBackground(int scale)
        {
            var width = graphPlace.ActualWidth;
            var height = graphPlace.ActualHeight;
            
            double wCenter = xMove + ( width / 2 ); 
            double hCenter = yMove + ( height / 2 );

            double xMargin = wCenter % scale;
            double yMargin = hCenter % scale;

            graphPlace.Children.Clear();

            for (double x = xMargin; x < width; x += scale)
            {
                Brush color = ((x >= wCenter) && (x < wCenter + scale) ? Brushes.SkyBlue : Brushes.DimGray);
                drawLineAbsolute(x, 0, x, height, color);
            }

            for (double y = yMargin; y < height; y += scale)
            {
                Brush color = ((y >= hCenter) && (y < hCenter + scale) ? Brushes.SkyBlue : Brushes.DimGray);
                drawLineAbsolute(0, y, width, y, color);
            }
        }

        private void graphPlaceReDraw()
        {
            graphPlaceBackground((int)graphScale.Value);
            if (graphDrawAlready) drawAlgorithm();
        }

        private void graphPlace_Loaded(object sender, RoutedEventArgs e)
        {
            graphPlaceReDraw();
        }

        private void graphPlace_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            graphPlaceReDraw();
        }

        private void graphScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            graphPlaceReDraw();
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            graphDrawAlready = true;
            graphPlaceReDraw();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            graphPlace.Children.Clear();
            graphPlaceBackground((int)graphScale.Value);
            graphDrawAlready = false;
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

                xMove = e.GetPosition(null).X - p.X;
                yMove = e.GetPosition(null).Y - p.Y;

                graphPlaceReDraw();
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
    }
}
