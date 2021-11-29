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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace PracticaFinal
{
   
    public partial class MainWindow : Window
    {

        ObservableCollection<Dia> dias;

        public MainWindow()
        {
            InitializeComponent();
            dias = new ObservableCollection<Dia>();
            dias.CollectionChanged += onDiasChange;
            chart.PropertyChanged += sizeChanged;
        }

        public void ShowAddCalories(object sender, RoutedEventArgs e)
        {
            
            AddCalories addCalories = new AddCalories();
            addCalories.Owner = this;
            addCalories.ShowDialog();
            if (addCalories.DialogResult == true)
            {
                DateTime fecha = addCalories.fecha_;
                String comida = addCalories.comida_;
                double calorias = addCalories.calorias_;

                IEnumerable<Dia> results = dias.Where(d => d.fecha.Equals(fecha));
                if (!results.Any())
                {
                    Dia d = new Dia(fecha, new Dictionary<String, double>() { { comida, calorias } });
                    d.PropertyChanged += onDiaChange;
                    dias.Add(d);
                }
                else
                {
                    foreach (Dia d in results)
                    {
                        if (d.Comidas.ContainsKey(comida)) MessageBox.Show("Ya existe esa comida");
                        else
                        {
                            Dictionary<String, double> dic = d.Comidas;
                            dic.Add(comida, calorias);
                            d.Comidas = dic;
                        }
                    }
                }    
            }
        }

        void onDiasChange(Object sender, NotifyCollectionChangedEventArgs e)
        {
            Dia d = dias.Last();
            int width = (chart.X2 - chart.X1) / 32;
            int x = chart.X1 + (width * d.fecha.Day);

            List<Line> lineas = d.getPolyline(chart.Y1, chart.Y2, width);
            
            foreach(Line l in lineas)
            {
                l.X1 = x;
                l.X2 = x;
                chart.Children.Add(l);
            }
        }

        void onDiaChange(Object sender, PropertyChangedEventArgs e)
        {
            Dia d = (Dia)sender;
            int width = (chart.X2 - chart.X1) / 32;
            int x = chart.X1 + (width * d.fecha.Day);

            List<Line> lineas = d.getPolyline(chart.Y1, chart.Y2, width);

            foreach (Line l in lineas)
            {
                l.X1 = x;
                l.X2 = x;
                chart.Children.Add(l);
            }
        }

        void sizeChanged(object sender,  PropertyChangedEventArgs e)
        {
            int width = (chart.X2 - chart.X1) / 32;

            foreach (Dia d in dias)
            {
                List<Line> lineas = d.getPolyline(chart.Y1, chart.Y2, width);
                int x = chart.X1 + (width * d.fecha.Day);
                foreach (Line l in lineas)
                {
                    l.X1 = x;
                    l.X2 = x;
                    chart.Children.Add(l);
                }
            }
        }
    }
}
