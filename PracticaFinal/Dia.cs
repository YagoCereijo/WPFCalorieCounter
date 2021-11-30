using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PracticaFinal
{
    public class Dia : INotifyPropertyChanged
    {
       
        private static List<String> nombreComidas = new List<String> {"DESAYUNO", "APERITIVO", "COMIDA", "MERIENDA", "CENA", "OTROS"};
        private static Dictionary<String, Brush> colorComidas = new Dictionary<String, Brush> {
            { "DESAYUNO", Brushes.Red },
            { "APERITIVO", Brushes.Yellow },
            { "COMIDA", Brushes.Green },
            { "MERIENDA", Brushes.Blue },
            { "CENA", Brushes.Purple },
            { "OTROS", Brushes.Pink } 
        };
        private static int maxCalories = 1500;
        private List<Comida> comidas = new List<Comida>();

        public double Calorias
        {
            get
            {
                double cal = 0;
                foreach (Comida c in Comidas) cal += c.Calorias;
                return cal;
            }

            set { Calorias = value; }
        }
        public DateTime Fecha { get; set; }
        public List<Comida> Comidas{
            get
            {
                return comidas;
            }
            set 
            {
               comidas = value; OnPropertyChanged("Comidas"); 
            }
        }
       
        public Dia(DateTime f, List<Comida> c)
        {
            Fecha = f;
            Comidas = c;
        }

        public List<Line> getPolyline(int minY, int maxY, int width)
        {
            List<Line> customPolyline = new List<Line>();
            int y = maxY;
            int height = maxY - minY;

            foreach(Comida c in Comidas)
            {               
                Line linea = new Line();
                linea.Y1 = y;
                y = y - ((int)c.Calorias * height / 6 / maxCalories);
                linea.Y2 = y;

                linea.StrokeThickness = width;
                linea.Stroke = colorComidas[c.Nombre];
     
                customPolyline.Add(linea);     
            }

            return customPolyline;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }

    public class Comida
    {
        public string Nombre { get; set; }
        public double Calorias { get; set; }
        public Comida(string n, double c)
        {
            Nombre = n;
            Calorias = c;
        }
    }

    
}
