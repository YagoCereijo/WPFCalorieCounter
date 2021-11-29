using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PracticaFinal
{
    public class Chart: Canvas
    {   
        public Chart()
        {
            this.Loaded += new RoutedEventHandler(Draw);
            this.SizeChanged += new SizeChangedEventHandler(Draw);
        }

        void Draw(object sender, RoutedEventArgs e)
        {
            this.Children.Clear();
            
            int widthPoints = (int)this.ActualWidth;
            int heightPoints = (int)this.ActualHeight;

            int y1 = heightPoints - heightPoints / 10;
            int y2 = y1 + heightPoints;
            int x1 = widthPoints / 10;
            int x2 = widthPoints - x1;


            Line xAxis = new Line();
            xAxis.X1 = x1;
            xAxis.Y1 = y1;
            xAxis.X2 = x2;
            xAxis.Y2 = y1;

            Line yAxis = new Line();
            yAxis.X1 = x1;
            yAxis.X2 = x1;
            yAxis.Y1 = y1;
            yAxis.Y2 = y2;

            xAxis.Stroke = Brushes.Black;
            yAxis.Stroke = Brushes.Black;

            this.Children.Add(xAxis);
            this.Children.Add(yAxis);

        }
    }
}
