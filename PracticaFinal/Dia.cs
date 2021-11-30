using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PracticaFinal
{
    public class Dia : INotifyPropertyChanged
    {
       
        public static List<String> nombreComidas = new List<String> {"DESAYUNO", "APERITIVO", "COMIDA", "MERIENDA", "CENA", "OTROS"};
        public static Dictionary<String, Brush> colorComidas = new Dictionary<String, Brush> {
            { "DESAYUNO", Brushes.Red },
            { "APERITIVO", Brushes.Yellow },
            { "COMIDA", Brushes.Green },
            { "MERIENDA", Brushes.Blue },
            { "CENA", Brushes.Purple },
            { "OTROS", Brushes.Pink } 
        };
        public static int maxCalories = 1500;
        public DateTime Fecha { get; set; }

        private Dictionary<String, double> comidas;

        public Dictionary<String, double> Comidas
        {
            get { return comidas; }
            set { comidas = value; OnPropertyChanged("Comidas"); }
        }

        public int Calorias
        {
            get
            {
                int cal = 0;
                foreach(int c in Comidas.Values)
                {
                    cal += c;
                }
                return cal;
            }

            set { Calorias = value; }
        }

        public Dia(DateTime f, Dictionary<String, double> c)
        {
            this.Fecha = f;
            this.comidas = c;
        }

        public List<Line> getPolyline(int minY, int maxY, int width)
        {
            List<Line> customPolyline = new List<Line>();
      
            int y = maxY;
            int height = maxY - minY;

            foreach(String n in nombreComidas)
            {
                if (comidas.ContainsKey(n))
                {
                    Line linea = new Line();
                    linea.Y1 = y;
                    y = y - ((int)comidas[n] * height / 6 / maxCalories);
                    linea.Y2 = y;

                    linea.StrokeThickness = width;
                    linea.Stroke = colorComidas[n];
     
                    customPolyline.Add(linea);
                }
            }

            return customPolyline;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

    }

    
}
