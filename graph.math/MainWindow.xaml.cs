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
        string[] algorithmLines;

        delegate double simpleFunction(double x);

        bool moveGraphPlace = false;
        bool graphDrawAlready = false;

        public MainWindow()
        {
            InitializeComponent();
            hightlightText = graphText.SelectionBrush;
            Draw.graphPlace = graphPlace;
            Draw.graphScale = graphScale;
            Error.graphText = graphText;

            Draw.graphPlaceIsReady = true;
        }

        string[] parseGraphText()
        {
            return graphText.Text.Split('\n');
        }

        string[] parseStrParam(string algorithmLine)
        {
            int start = algorithmLine.IndexOf('(') + 1;
            int end = algorithmLine.IndexOf(')') - algorithmLine.IndexOf('(') - 1;

            if (start == -1 || end == -1) return new string[] { };

            string[] paramLines = Regex.Split(algorithmLine.Substring(start, end), ",|:|->");

            string[] paramLinesT = Array.ConvertAll(paramLines, n => n.Trim());

            return paramLinesT;
        }

        int[] parseParam(string algorithmLine)
        {
            string[] paramLines = parseStrParam(algorithmLine);

            int param;

            for (int a = 0; a < paramLines.Length; a++)
                if (!int.TryParse(paramLines[a], out param))
                    return new int[] { };

            return Array.ConvertAll(paramLines, n => int.Parse(n));
        }

        private void graphPlaceReDraw()
        {
            Draw.graphPlaceBackground((int)graphScale.Value);
            if (graphDrawAlready) drawAlgorithm();
        }

        bool drawAlgorithmLine(string algorithmLine, int line)
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

                if ( Block.allBlocks[startLine].skipThisBlock != true )
                    Block.allBlocks[line + 1].skipThisBlock = true;

                return true;
            }

            else if (algorithmLine.IndexOf("repeat") > -1)
            {
                string[] p = parseStrParam(algorithmLine);

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
                int[] p = parseParam(algorithmLine);

                if (p.Length != 4) return false;

                if (varName != "")
                    Vector.createNewVector(varName, p);

                return Draw.drawLine(p, Brushes.White);
            }

            else if (algorithmLine.IndexOf("sum") > -1)
                return Draw.drawSumVector(parseStrParam(algorithmLine));

            else if (algorithmLine.IndexOf("point") > -1)
            {
                int[] p = parseParam(algorithmLine);

                if (p.Length != 2) return false;

                return Draw.drawPoint(p, Brushes.Red);
            }

            else if (algorithmLine.IndexOf("sin") > -1)
                return Draw.drawSimpleFuntion(Math.Sin, parseParam(algorithmLine));

            else if (algorithmLine.IndexOf("cos") > -1)
                return Draw.drawSimpleFuntion(Math.Cos, parseParam(algorithmLine));

            else if (algorithmLine.IndexOf("tg") > -1)
                return Draw.drawSimpleFuntion(Math.Tan, parseParam(algorithmLine));

            else
                return false;
        }

        bool drawAlgorithm()
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

                Draw.xMove = e.GetPosition(null).X - p.X;
                Draw.yMove = e.GetPosition(null).Y - p.Y;

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

        private void buttonPlay_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            graphDrawAlready = true;
            algorithmLines = parseGraphText();
            graphPlaceReDraw();
        }

        private void buttonClear_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            graphPlace.Children.Clear();
            Draw.graphPlaceBackground((int)graphScale.Value);
            graphDrawAlready = false;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            graphText.Height = graphGrid.ActualHeight - 135;

            double graphScaleTop = graphText.Height + graphText.Margin.Top + 12;
            graphScale.Margin = new Thickness(8, graphScaleTop, 12, 8);
        }
    }
}
