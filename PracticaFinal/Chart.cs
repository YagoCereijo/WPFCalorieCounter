using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PracticaFinal
{
    public class Chart : Canvas, INotifyPropertyChanged
    {
        private int x1, x2, y1, y2;

        public int X1
        {
            get { return x1; }
            set { x1 = value; }
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

        public void DrawMonth(List<Dia> dias, int mes, int anio)
        {
            Draw();
            int width = (X2 - X1) / 32;
            int y = Y2;
            int height = Y2 - Y1;
            for (int i = 1500; i <= 1500 * 6; i += 1500)
            {
                TextBlock label = new TextBlock();
                label.Text = i.ToString() + " -";
                label.Foreground = Brushes.Black;
                Canvas.SetLeft(label, X1 - 30);
                Canvas.SetBottom(label, Y1 + i * height / (1500 * 6) - 6);
                Children.Add(label);
            }

            List<Dia> select = dias.FindAll(d => d.Fecha.Date.Month == mes && d.Fecha.Date.Year == anio);

            foreach (Dia d in select)
            {
                int x = X1 + (width * d.Fecha.Day);

                List<Line> lineas = d.getPolyline(Y1, Y2, width);
                EventArgs e = new EventArgs();
                

                foreach (Line l in lineas)
                {
                    l.X1 = x;
                    l.X2 = x;

                    Children.Add(l);
                }

                TextBlock label = new TextBlock();
                label.Text = d.Fecha.Day.ToString();
                label.Foreground = Brushes.Black;
                Children.Add(label);
                Canvas.SetLeft(label, x - 6);
                Canvas.SetTop(label, Y2);

            }
        }

        public void DrawDay(List<Comida> comidas)
        {
            Draw();
            int width = (X2 - X1) / 6;
            int height = Y2 - Y1;
            int i = 1;

            for (int j = 300; j <= 1500; j += 300)
            {
                TextBlock label = new TextBlock();
                label.Text = j.ToString();
                label.Foreground = Brushes.Black;
                Canvas.SetLeft(label, X1 - 30);
                Canvas.SetBottom(label, Y1 + j * height / (1500) - 6);
                Children.Add(label);
            }
            
            foreach (Comida c in comidas)
            {
                Line linea = c.getLine(Y1, Y2, width);
                int x = (X1 + (width * i)) - (width / 2) + 1;
                linea.X1 = x;
                linea.X2 = x;
                Children.Add(linea);
                i++;

                TextBlock label = new TextBlock();
                label.Text = c.Nombre;
                label.Foreground = Brushes.Black;
                Children.Add(label);
                Canvas.SetLeft(label, x - width / 3);
                Canvas.SetTop(label, Y2);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
