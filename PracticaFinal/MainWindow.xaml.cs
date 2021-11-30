using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace PracticaFinal
{ 
    public partial class MainWindow : Window
    {
        ObservableCollection<Dia> dias;
        ListaCalorias listaCalorias = new ListaCalorias();

        public MainWindow()
        {
            InitializeComponent();
            dias = new ObservableCollection<Dia>();
            dias.CollectionChanged += onDiasChange;
            dias.CollectionChanged += onDiasManageInnerChange;
            chart.PropertyChanged += sizeChanged;
        }

        public void ShowDayList(object sender, RoutedEventArgs e)
        {
            listaCalorias.lista.ItemsSource = dias;
            listaCalorias.Owner = this;
            listaCalorias.Show();
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

                IEnumerable<Dia> results = dias.Where(d => d.Fecha.Equals(fecha));
                if (!results.Any())
                {
                    Dia d = new Dia(fecha, new Dictionary<String, double>() { { comida, calorias } });
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
            int x = chart.X1 + (width * d.Fecha.Day);

            List<Line> lineas = d.getPolyline(chart.Y1, chart.Y2, width);
            
            foreach(Line l in lineas)
            {
                l.X1 = x;
                l.X2 = x;
                chart.Children.Add(l);
            }
        }

        void onDiasManageInnerChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += onDiaChange;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= onDiaChange;
                }
            }
        }

        void onDiaChange(Object sender, PropertyChangedEventArgs e)
        {
            Dia d = (Dia)sender;
            int width = (chart.X2 - chart.X1) / 32;
            int x = chart.X1 + (width * d.Fecha.Day);

            List<Line> lineas = d.getPolyline(chart.Y1, chart.Y2, width);

            foreach (Line l in lineas)
            {
                l.X1 = x;
                l.X2 = x;
                chart.Children.Add(l);
            }

            listaCalorias.lista.Items.Refresh();


        }

        void sizeChanged(object sender,  PropertyChangedEventArgs e)
        {
            int width = (chart.X2 - chart.X1) / 32;

            foreach (Dia d in dias)
            {
                List<Line> lineas = d.getPolyline(chart.Y1, chart.Y2, width);
                int x = chart.X1 + (width * d.Fecha.Day);
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
