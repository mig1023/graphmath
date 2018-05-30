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

        delegate double simpleFunction(double x);

        bool moveGraphPlace = false;
        bool graphDrawAlready = false;
        double xMove = 0;
        double yMove = 0;

        public MainWindow()
        {
            InitializeComponent();
            hightlightText = graphText.SelectionBrush;
        }

        private int getValueMax(bool negativeNumber = false, bool isY = false)
        {
            int scale = (int)graphScale.Value;
            double size = ( isY ? graphPlace.ActualHeight : graphPlace.ActualWidth );

            int centerPoint = (int)(getCenter(size, xMove) / scale);

            if (isY) negativeNumber = !negativeNumber;

            if (!negativeNumber)
                return (int)(size / scale) - centerPoint;
            else
                return -1 * (centerPoint + 1);
        }

        private double getCenter(double actualSize, double xyMove)
        {
            double center = xyMove + (actualSize / 2);
            int scale = (int)graphScale.Value;

            double centerPoint = 0;

            for (double x = center % scale; x < actualSize; x += scale)
                if ((x >= center) && (x < center + scale))
                    centerPoint = x;

            return centerPoint;
        }

        private double getPoint(double xyPoint, double actualSize, double xyMove, bool isY)
        {
            double centerPoint = getCenter(actualSize, xyMove);
            int scale = (int)graphScale.Value;
            double newPoint = centerPoint + ((xyPoint * scale) * (isY ? 1 : -1));

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

        private bool drawPoint(double x, double y, Brush color, int width = 2)
        {
            drawPointAbsolute(
                x: getXPoint(x),
                y: getYPoint(y),
                color: color,
                width: width
            );

            return true;
        }

        private bool drawPoint(int[] coordinate, Brush color, int width = 2)
        {
            drawPoint(
                x: coordinate[0],
                y: coordinate[1],
                color: color,
                width: width
            );

            return true;
        }

        private bool drawPointAbsolute(double x, double y, Brush color, int width = 2)
        {
            Point point = new Point(x, y);
            Ellipse elipse = new Ellipse();

            elipse.Width = width;
            elipse.Height = width;

            elipse.StrokeThickness = 2;
            elipse.Stroke = color;
            elipse.Margin = new Thickness(point.X - (width / 2), point.Y - (width / 2), 0, 0);

            graphPlace.Children.Add(elipse);

            return true;
        }

        private bool drawLine(int[] coordinate, Brush color, int width = 1, bool dash = false)
        {
            drawLine(
                x1: coordinate[0],
                y1: coordinate[1],
                x2: coordinate[2],
                y2: coordinate[3],
                color: color,
                width: width,
                dash: dash
            );

            return true;
        }

        private bool drawLine(double x1, double y1, double x2, double y2,
            Brush color, int width = 1, bool dash = false)
        {
            drawLineAbsolute(
                x1: getXPoint(x1),
                y1: getYPoint(y1),
                x2: getXPoint(x2),
                y2: getYPoint(y2),
                color: color,
                width: width,
                dash: dash
            );

            return true;
        }

        private bool drawLineAbsolute(double x1, double y1, double x2, double y2,
            Brush color, int width = 1, bool dash = false)
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

            if (dash)
            {
                line.StrokeDashArray = new DoubleCollection() { 4 };
            }

            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            graphPlace.Children.Add(line);

            return true;
        }

        string[] parseGraphText()
        {
            return graphText.Text.Split('\n');
        }

        int[] parseParam(string algorithmLine)
        {
            int start = algorithmLine.IndexOf('(') + 1;
            int end = algorithmLine.IndexOf(')') - algorithmLine.IndexOf('(') - 1;

            if (start == -1 || end == -1) return new int[] { };

            string[] paramLines = Regex.Split(algorithmLine.Substring(start, end), ",|:");

            return Array.ConvertAll(paramLines, n => int.Parse(n));
        }

        string parseVariable(string algorithmLine)
        {
            int equalitySign = algorithmLine.IndexOf('=');

            if (equalitySign == -1) return "";
            
            return algorithmLine.Substring(0, equalitySign).Trim();
        }

        void algorithmError(int start, int end)
        {
            graphText.SelectionBrush = Brushes.Red;
            graphText.Focus();
            graphText.Select(start, end);
        }

        bool drawSimpleFuntion(simpleFunction function, int[] param)
        {
            if (param.Length == 0)
                for (double x = getValueMax(negativeNumber: true); x < getValueMax(); x += 0.05)
                    drawPoint(x, function.Invoke(x), Brushes.White, 1);

            else if (param.Length == 1)
                drawPoint(param[0], function.Invoke(param[0]), Brushes.Red);

            else return false;

            return true;
        }

        bool drawSumVector(int[] param)
        {
            if (param.Length != 2) return false;

            Vector v1 = new Vector();
            Vector v2 = new Vector();

            foreach (Vector v in Vector.allVectors)
            {
                if (v.variableName == param[0].ToString()) v1 = v;
                if (v.variableName == param[1].ToString()) v2 = v;
            }

            double x1 = v1.x1 + v2.x1;
            double y1 = v1.y1 + v2.y1;
            double x2 = v1.x2 + v2.x2;
            double y2 = v1.y2 + v2.y2;

            drawLine(v1.x2, v1.y2, x2, y2, Brushes.Red, dash: true);
            drawLine(v2.x2, v2.y2, x2, y2, Brushes.Red, dash: true);

            drawLine(v1.x1, v1.y1, x1, y1, Brushes.Red, dash: true);
            drawLine(v2.x1, v2.y1, x1, y1, Brushes.Red, dash: true);

            drawLine(x1, y1, x2, y2, Brushes.Red);

            return true;
        }

        bool drawAlgorithmLine(string algorithmLine)
        {
            int[] p = parseParam(algorithmLine);

            if (algorithmLine.IndexOf("vector") > -1)
            {
                if (p.Length != 4) return false;

                string varName = parseVariable(algorithmLine);

                if (varName != "")
                    Vector.createNewVector(varName, p);

                return drawLine(p, Brushes.White);
            }

            if (algorithmLine.IndexOf("sum") > -1)
            {
                return drawSumVector(p);
            }

            else if (algorithmLine.IndexOf("point") > -1)
            {
                if (p.Length != 2) return false;

                return drawPoint(p[0], p[1], Brushes.Red);
            }

            else if (algorithmLine.IndexOf("sin") > -1)
            {
                return drawSimpleFuntion(Math.Sin, p);
            }

            else if (algorithmLine.IndexOf("cos") > -1)
            {
                return drawSimpleFuntion(Math.Cos, p);
            }

            else if (algorithmLine.IndexOf("tg") > -1)
            {
                return drawSimpleFuntion(Math.Tan, p);
            }

            else
                return false;
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
