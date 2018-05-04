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

        void graphPlaceBackground(int scale)
        {
            var width = graphPlace.ActualWidth;
            var height = graphPlace.ActualHeight;

            double wcenter = width / 2; 
            double hcenter = height / 2;

            graphPlace.Children.Clear();

            for (int x = scale; x < width; x += scale)
                if ((x >= wcenter) && (x < wcenter + scale))
                    AddLineToBackground(x, 0, x, height, true);
                else
                    AddLineToBackground(x, 0, x, height);

            for (int y = scale; y < height; y += scale)
                if ((y > hcenter) && (y <hcenter + scale) )
                    AddLineToBackground(0, y, width, y, true);
                else
                    AddLineToBackground(0, y, width, y);
        }

        void AddLineToBackground(double x1, double y1, double x2, double y2, bool coordinate = false)
        {
            var line = new Line()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = (coordinate ? Brushes.SkyBlue : Brushes.DarkGray ),
                StrokeThickness = (coordinate ? 2 : 1),
                SnapsToDevicePixels = true
            };
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            graphPlace.Children.Add(line);
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
    }
}
