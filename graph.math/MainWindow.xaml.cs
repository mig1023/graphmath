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
using System.Text.RegularExpressions;

namespace graph.math
{
    public partial class MainWindow : Window
    {
        Point p;
        Brush hightlightText;

        bool moveGraphPlace = false;
        bool graphDrawAlready = false;
        double xMove = 0;
        double yMove = 0;

        public MainWindow()
        {
            InitializeComponent();
            hightlightText = graphText.SelectionBrush;
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

        private void drawPoint(double x, double y, Brush color, int width = 2)
        {
            drawPointAbsolute(
                x: getXPoint(x),
                y: getYPoint(y),
                color: color,
                width: width
           );
        }

        private void drawPointAbsolute(double x, double y, Brush color, int width = 2)
        {
            Point point = new Point(x, y);
            Ellipse elipse = new Ellipse();

            elipse.Width = width;
            elipse.Height = width;

            elipse.StrokeThickness = 2;
            elipse.Stroke = color;
            elipse.Margin = new Thickness(point.X - (width / 2), point.Y - (width / 2), 0, 0);

            graphPlace.Children.Add(elipse);
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

        int[] parseParam(string algorithmLine)
        {
            int start = algorithmLine.IndexOf('(') + 1;
            int end = algorithmLine.IndexOf(')') - algorithmLine.IndexOf('(') - 1;

            string[] paramLines = Regex.Split(algorithmLine.Substring(start, end), ",|:");

            return Array.ConvertAll(paramLines, n => int.Parse(n));
        }

        void algorithmError(int start, int end)
        {
            graphText.SelectionBrush = Brushes.Red;
            graphText.Focus();
            graphText.Select(start, end);
        }

        bool drawAlgorithmLine(string algorithmLine)
        {
            if (algorithmLine.IndexOf("line") > -1)
            {
                int[] param = parseParam(algorithmLine);

                if (param.Length != 4) return false;

                drawLine(param[0], param[1], param[2], param[3], Brushes.White);
            }

            if (algorithmLine.IndexOf("point") > -1)
            {
                int[] param = parseParam(algorithmLine);

                if (param.Length != 2) return false;

                drawPoint(param[0], param[1], Brushes.Red);
            }

            if (algorithmLine.IndexOf("sine") > -1)
            {
                for (double x = -10; x < 10; x += 0.01)
                {
                    double y = Math.Sin(x);
                    drawPoint(x, y, Brushes.White, 1);
                }
            }

            return true;
        }

        void drawAlgorithm()
        {
            string[] algorithmLines = parseGraphText();

            int currentLineStart = 0;

            foreach(string algorithmLine in algorithmLines)
            {
                if (!drawAlgorithmLine(algorithmLine))
                    algorithmError(currentLineStart, algorithmLine.Length);

                currentLineStart += algorithmLine.Length + 1;
            }
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

        private void graphText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            graphText.SelectionBrush = hightlightText;
        }
    }
}
