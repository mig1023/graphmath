using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace graph.math
{
    class Draw
    {
        public delegate double simpleFunction(double x);

        public static Canvas graphPlace;
        public static Slider graphScale;
        public static MainWindow main;
        public static double xMove = 0;
        public static double yMove = 0;

        public static bool graphPlaceIsReady = false;

        public static string[] algorithmLines;

        private static int getValueMax(bool negativeNumber = false, bool isY = false)
        {
            int scale = (int)graphScale.Value;
            double size = (isY ? graphPlace.ActualHeight : graphPlace.ActualWidth);

            int centerPoint = (int)(getCenter(size, xMove) / scale);

            if (isY) negativeNumber = !negativeNumber;

            if (!negativeNumber)
                return (int)(size / scale) - centerPoint;
            else
                return -1 * (centerPoint + 1);
        }

        public static double getCenter(double actualSize, double xyMove)
        {
            double center = xyMove + (actualSize / 2);
            int scale = (int)graphScale.Value;

            double centerPoint = 0;

            for (double x = center % scale; x < actualSize; x += scale)
                if ((x >= center) && (x < center + scale))
                    centerPoint = x;

            return centerPoint;
        }

        private static double getPoint(double xyPoint, double actualSize, double xyMove, bool isY)
        {
            double centerPoint = getCenter(actualSize, xyMove);
            int scale = (int)graphScale.Value;
            double newPoint = centerPoint + ((xyPoint * scale) * (isY ? -1 : 1));

            return newPoint;
        }

        public static double getXPoint(double xPoint)
        {
            return getPoint(
                xyPoint: xPoint,
                actualSize: graphPlace.ActualWidth,
                xyMove: xMove,
                isY: false
            );
        }

        public static double getYPoint(double yPoint)
        {
            return getPoint(
                xyPoint: yPoint,
                actualSize: graphPlace.ActualHeight,
                xyMove: yMove,
                isY: true
            );
        }

        public static bool drawPointAbsolute(double x, double y, Brush color, int width = 2)
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

        public static bool drawPoint(double x, double y, Brush color, int width = 2)
        {
            drawPointAbsolute(
                x: getXPoint(x),
                y: getYPoint(y),
                color: color,
                width: width
            );

            return true;
        }

        public static bool drawPoint(int[] coordinate, Brush color, int width = 2)
        {
            drawPoint(
                x: coordinate[0],
                y: coordinate[1],
                color: color,
                width: width
            );

            return true;
        }

        public static bool drawLine(int[] coordinate, Brush color, int width = 1, bool dash = false)
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

        public static bool drawLine(double x1, double y1, double x2, double y2,
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

        public static bool drawLineAbsolute(double x1, double y1, double x2, double y2,
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

            line.MouseMove += new MouseEventHandler(main.graphPlace_MouseMove);
            line.MouseUp += new MouseButtonEventHandler(main.graphPlace_MouseUp);
            line.MouseDown += new MouseButtonEventHandler(main.graphPlace_MouseDown);

            graphPlace.Children.Add(line);

            return true;
        }

        public static bool drawSimpleFuntion(simpleFunction function, int[] param)
        {
            if (param.Length == 0)
                for (double x = getValueMax(negativeNumber: true); x < getValueMax(); x += 0.05)
                    drawPoint(x, function.Invoke(x), Brushes.White, 1);

            else if (param.Length == 1)
                drawPoint(param[0], function.Invoke(param[0]), Brushes.Red);

            else return false;

            return true;
        }

        public static bool drawSumVector(string[] param)
        {
            if (param.Length != 2) return false;

            Vector v1 = new Vector();
            v1 = Vector.allVectors[param[0].ToString()];

            Vector v2 = new Vector();
            v2 = Vector.allVectors[param[1].ToString()];

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

        public static void graphPlaceBackground(int scale)
        {
            if (!graphPlaceIsReady) return;

            var width = graphPlace.ActualWidth;
            var height = graphPlace.ActualHeight;

            double wCenter = xMove + (width / 2);
            double hCenter = yMove + (height / 2);

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

        public static int getValueMax(Canvas graphPlace, Slider graphScale, double xMove, double yMove,
            bool negativeNumber = false, bool isY = false)
        {
            int scale = (int)graphScale.Value;
            double size = (isY ? graphPlace.ActualHeight : graphPlace.ActualWidth);

            int centerPoint = (int)(getCenter(size, xMove) / scale);

            if (isY) negativeNumber = !negativeNumber;

            if (!negativeNumber)
                return (int)(size / scale) - centerPoint;
            else
                return -1 * (centerPoint + 1);
        }

        public static bool drawAlgorithmLine(string algorithmLine, int line)
        {
            algorithmLine = Var.replaceVariable(algorithmLine);

            string varName = Var.parseVariable(algorithmLine);

            if (Regexp.Check(@"^[\t\r\n\s]*$", algorithmLine))
                return true;

            else if (algorithmLine.IndexOf("if") > -1)
            {
                int conditionResult = Conditional.check(algorithmLine);

                if (conditionResult == 0)
                    return false;
                else if (conditionResult == 2)
                    Block.allBlocks[line + 1].skipThisBlock = true;
                else
                    Block.allBlocks[line + 1].skipThisBlock = false;

                return true;
            }

            else if (algorithmLine.IndexOf("else") > -1)
            {
                int startLine = Block.startOfThisBlock(line);

                if (startLine == -1)
                    return false;

                if (Block.allBlocks[startLine].skipThisBlock == true)
                    Block.allBlocks[line + 1].skipThisBlock = false;
                else
                    Block.allBlocks[line + 1].skipThisBlock = true;

                return true;
            }

            else if (algorithmLine.IndexOf("repeat") > -1)
            {
                string[] p = Parse.parseStrParam(algorithmLine);

                if (p.Length != 3) return false;

                return Loop.createNewLoop(
                    varName: p[0],
                    startLine: Block.allBlocks[line + 1].startLine,
                    endLine: Block.allBlocks[line + 1].endLine,
                    currentVar: int.Parse(p[1]),
                    endStatment: int.Parse(p[2])
                );
            }

            else if (Var.isIncrement(algorithmLine))
                return Var.Increment(varName, algorithmLine);

            else if (Var.isVariable(algorithmLine))
                return Var.createNewVar(varName, algorithmLine);

            else if (algorithmLine.IndexOf("vector") > -1)
            {
                int[] p = Parse.parseParam(algorithmLine);

                if (p.Length != 4) return false;

                if (varName != "")
                    Vector.createNewVector(varName, p);

                return Draw.drawLine(p, Brushes.White);
            }

            else if (algorithmLine.IndexOf("sum") > -1)
                return Draw.drawSumVector(Parse.parseStrParam(algorithmLine));

            else if (algorithmLine.IndexOf("point") > -1)
            {
                int[] p = Parse.parseParam(algorithmLine);

                if (p.Length != 2) return false;

                return Draw.drawPoint(p, Brushes.Red);
            }

            else if (algorithmLine.IndexOf("sin") > -1)
                return Draw.drawSimpleFuntion(Math.Sin, Parse.parseParam(algorithmLine));

            else if (algorithmLine.IndexOf("cos") > -1)
                return Draw.drawSimpleFuntion(Math.Cos, Parse.parseParam(algorithmLine));

            else if (algorithmLine.IndexOf("tg") > -1)
                return Draw.drawSimpleFuntion(Math.Tan, Parse.parseParam(algorithmLine));

            else
                return false;
        }

        public static bool drawAlgorithm()
        {
            Vector.allVectors.Clear();
            Var.allVars.Clear();
            Block.allBlocks.Clear();
            Loop.allLoops.Clear();

            Block.algorithmLines = algorithmLines;
            Block.lineSeparator(algorithmLines);
            Error.algorithmLinesCalc(algorithmLines);

            for (int line = 0; line < algorithmLines.Length; line++)
            {
                string algLine = algorithmLines[line];

                int comment = algorithmLines[line].IndexOf("//");
                if (comment > -1)
                    algLine = algorithmLines[line].Remove(comment);

                if (!drawAlgorithmLine(algLine, line))
                    return Error.algorithmError(line);

                if (Block.allBlocks.ContainsKey(line + 1))
                    if (Block.allBlocks[line + 1].skipThisBlock)
                        line = Block.allBlocks[line + 1].endLine;

                line = Loop.returnLoop(line);
            }

            return true;
        }


    }
}
