using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace graph.math
{
    public partial class MainWindow : Window
    {
        Point p;
        Brush hightlightText;

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
        
        private void graphPlaceReDraw()
        {
            Draw.graphPlaceBackground((int)graphScale.Value);
            if (graphDrawAlready) Draw.drawAlgorithm();
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
            Draw.algorithmLines = graphText.Text.Split('\n');
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
