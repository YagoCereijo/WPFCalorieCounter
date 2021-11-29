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
    public class MonthlyChart: Canvas
    {

        public Collection<Comida> Comidas { get; set; } = new Collection<Comida>();
        
        static public List<string> nombreComidas;
        static public Dictionary<string, Brush> colorComida;
        public MonthlyChart()
        {
            this.Loaded += new RoutedEventHandler(Draw);
            this.SizeChanged += new SizeChangedEventHandler(Draw);
            nombreComidas = new List<string>{"DESAYUNO", "APERITIVO", "COMIDA", "MERIENDA", "CENA", "OTROS"};
            colorComida = new Dictionary<string, Brush>
            {
                {nombreComidas[0], Brushes.Yellow },
                {nombreComidas[1], Brushes.Green },
                {nombreComidas[2], Brushes.Blue },
                {nombreComidas[3], Brushes.Red },
                {nombreComidas[4], Brushes.Purple },
                {nombreComidas[5], Brushes.Pink }
            };

        }

        void Draw(object sender, RoutedEventArgs e)
        {
            this.Children.Clear();
            int widthPoints = (int)this.ActualWidth;
            int heightPoints = (int)this.ActualHeight;
            int yAnchor = heightPoints - heightPoints / 10;
            int xAnchor = widthPoints / 10;
            List<DateTime> days = getDays(2021, 11);

            Line xAxis = new Line();
            xAxis.X1 = xAnchor;
            xAxis.Y1 = yAnchor;
            xAxis.X2 = widthPoints - widthPoints / 10;
            xAxis.Y2 = yAnchor;

            Line yAxis = new Line();
            yAxis.X1 = xAnchor;
            yAxis.X2 = xAnchor;
            yAxis.Y1 = yAnchor;
            yAxis.Y2 = heightPoints / 10;

            int frameWidth = (int)(xAxis.X2 - xAxis.X1);
            int frameHeight = (int)(yAxis.Y2 - yAxis.Y1);


            foreach(DateTime d in days)
            {
                int x = xAnchor + ((frameWidth / days.Count) * d.Day);
                int y = yAnchor;
                int i;
     
                List<Comida> comidasDia = new List<Comida>();

                foreach(Comida c in Comidas.Where(c => c.fecha.Equals(d))) comidasDia.Add(c);
                
                foreach(string s in nombreComidas)
                {
                    i = comidasDia.FindIndex(c => c.comida.Equals(s));
                    if (i != -1)
                    {
                        drawComida(comidasDia[i], x, y, frameHeight, frameWidth/days.Count);
                        y = y - (((int)comidasDia[i].calorias * y) / 6 / 255);
                    }
                }
            }

            xAxis.Stroke = Brushes.Black;
            yAxis.Stroke = Brushes.Black;
            
            this.Children.Add(xAxis);
            this.Children.Add(yAxis);

        }

        public void drawComida(Comida c, int x, int y, int frameHeight, int width)
        {
            Line cAxis = new Line();
            cAxis.X1 = x;
            cAxis.Y1 = y;
            cAxis.X2 = x;
            cAxis.Y2 = y - (((int)c.calorias * y) / 6 / 255);

            cAxis.Stroke = colorComida[c.comida];
            cAxis.StrokeThickness = width;

            this.Children.Add(cAxis);
        }

        public static List<DateTime> getDays(int year, int month)
        {
            var dates = new List<DateTime>();
            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates;
        }

        public void addData(Collection<Comida> c)
        {
            this.Comidas = c;
            Draw(this, new RoutedEventArgs());
        }
    }
}
