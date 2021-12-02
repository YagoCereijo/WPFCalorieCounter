using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PracticaFinal
{
    public class Chart: Canvas, INotifyPropertyChanged
    {
        private int x1, x2, y1, y2;

        public int X1
        {
            get { return x1; }
            set { x1 = value;}
        }

        public int X2
        {
            get { return x2; }
            set { x2 = value; OnPropertyChanged("X2"); }
        }

        public int Y1
        {
            get { return y1; }
            set { y1 = value; }
        }

        public int Y2
        {
            get { return y2; }
            set { y2 = value; OnPropertyChanged("Y2"); }
        }

        public Chart()
        {
            Loaded += Update;
            SizeChanged += Update;
        }

        void Update(object sender, RoutedEventArgs e)
        {
            Draw();
        }

        public void Draw()
        {
            this.Children.Clear();

            int widthPoints = (int)this.ActualWidth;
            int heightPoints = (int)this.ActualHeight;

            Y1 = heightPoints / 10;
            Y2 = heightPoints - y1;
            X1 = widthPoints / 10;
            X2 = widthPoints - x1;


            Line xAxis = new Line();
            xAxis.X1 = X1;
            xAxis.Y1 = Y2;
            xAxis.X2 = X2;
            xAxis.Y2 = Y2;

            Line yAxis = new Line();
            yAxis.X1 = X1;
            yAxis.X2 = X1;
            yAxis.Y1 = Y1;
            yAxis.Y2 = Y2;


            xAxis.Stroke = Brushes.Black;
            yAxis.Stroke = Brushes.Black;

            this.Children.Add(xAxis);
            this.Children.Add(yAxis);
        }

        public void DrawMonth(List<Dia> dias)
        {
            Draw();
            int width = (X2 - X1) / 32;

            foreach (Dia d in dias)
            {
                int x = X1 + (width * d.Fecha.Day);

                List<Line> lineas = d.getPolyline(Y1, Y2, width);

                foreach (Line l in lineas)
                {
                    l.X1 = x;
                    l.X2 = x;
                    Children.Add(l);
                }
            }
        }

        public void DrawDay(List<Comida> comidas)
        {
            Draw();
            int width = (X2 - X1) / 6;
            int i = 1;

            foreach (Comida c in comidas)
            {
                Line linea = c.getLine(Y1, Y2, width);
                int x = (X1 + (width * i)) - (width / 2) + 1;
                linea.X1 = x;
                linea.X2 = x;
                Children.Add(linea);
                i++;
            }
        }





        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
