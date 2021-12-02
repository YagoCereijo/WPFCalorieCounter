using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace PracticaFinal
{ 
    public partial class MainWindow : Window
    {
        ObservableCollection<Dia> dias;
        ListaCalorias listaCalorias;
        Dia diaActual;

        public MainWindow()
        {
            InitializeComponent();
            dias = new ObservableCollection<Dia>();
            dias.CollectionChanged += mesChart;
            dias.CollectionChanged += onDiasManageInnerChange;

            monthlyChart.PropertyChanged += sizeChanged;
            dailyChart.PropertyChanged += sizeChanged;
        }



        // WINDOWS RAISE EVENTS

        public void ShowDayList(object sender, RoutedEventArgs e)
        {
            listaCalorias = new ListaCalorias();
            listaCalorias.lista.ItemsSource = dias;
            listaCalorias.Owner = this;
            listaCalorias.lista.SelectionChanged += diaChartChanged;
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

                IEnumerable<Dia> resultsDia = dias.Where(d => d.Fecha.Equals(fecha));
                if (!resultsDia.Any())
                {
                    Dia d = new Dia(fecha, new ObservableCollection<Comida>{ new Comida(comida, calorias) });
                    dias.Add(d);
                }
                else
                {
                    bool exists = false;
                    foreach (Dia d in resultsDia)
                    {
                        foreach (Comida c in d.Comidas)
                            if (c.Nombre.Equals(comida))
                                exists = true;

                        if (exists) MessageBox.Show("Ya existe esa comida");
                        else
                        {
                            
                            d.Comidas.Add(new Comida(comida, calorias));
                        }
                    }
                }    
            }
        }




        // MANAGEMENT OF THE CHART

        void onDiasManageInnerChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Dia item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += mesChart;
                    item.Comidas.CollectionChanged += diaChart;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Dia item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= mesChart;
                    item.Comidas.CollectionChanged -= diaChart;
                }
            }
        }

        void mesChart(Object sender, EventArgs e){ monthlyChart.DrawMonth(dias.ToList()); }
        
        void sizeChanged(object sender,  PropertyChangedEventArgs e)
        {
            monthlyChart.DrawMonth(dias.ToList());
            if(dailyChart.Visibility == Visibility.Visible) { dailyChart.DrawDay(diaActual.Comidas.ToList()); }
        }
        void diaChartChanged(object sender, EventArgs e)
        {
            monthlyChart.Visibility = Visibility.Hidden;
            dailyChart.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            diaActual = (Dia)((ListView)sender).SelectedItem;
            dailyChart.DrawDay(diaActual.Comidas.ToList());
        }

        void diaChart(object sender, EventArgs e)
        {
            if(dailyChart.Visibility == Visibility.Visible)
            {
                dailyChart.DrawDay(diaActual.Comidas.ToList());
            }
            
        }

        private void backToMonthly(object sender, RoutedEventArgs e)
        {
            monthlyChart.Visibility = Visibility.Visible;
            dailyChart.Visibility = Visibility.Hidden;
            backButton.Visibility = Visibility.Hidden;
        }
    }
}
