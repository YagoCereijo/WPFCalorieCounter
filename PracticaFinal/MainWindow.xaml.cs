using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Text.Json;
using System.IO;

namespace PracticaFinal
{ 
    public partial class MainWindow : Window
    {
        ObservableCollection<Dia> Dias { get; set; }
        ListaCalorias listaCalorias;
        Dia diaActual;
        enum Meses { Enero = 1, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre }
        Meses mesActual;
        int anioActual;
        string path = AppDomain.CurrentDomain.BaseDirectory + "data.json";

        public MainWindow()
        {
            InitializeComponent();
            mesActual = (Meses)(DateTime.Today.Month);
            anioActual = (int)(DateTime.Today.Year);
            month.Content = mesActual.ToString();
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                Dias = JsonSerializer.Deserialize<ObservableCollection<Dia>>(jsonString);
            }
            else
            {
                Dias = new ObservableCollection<Dia>();
            }

            Dias.CollectionChanged += mesChart;
            Dias.CollectionChanged += onDiasManageInnerChange;
            monthlyChart.SizeChanged += sizeChanged;
            monthlyChart.Loaded += mesChart;
            dailyChart.SizeChanged += sizeChanged;
        }



        // BUTTON EVENTS

        public void ShowDayList(object sender, RoutedEventArgs e)
        {
            listaCalorias = new ListaCalorias(Dias);
            listaCalorias.Owner = this;
            listaCalorias.listaDia.SelectionChanged += diaChartChanged;
            listaCalorias.botonBorrarComida.Click += diaChartChanged;
            listaCalorias.Show();
        }
        public void ShowAddCalories(object sender, RoutedEventArgs e)
        {       
            AddCalories addCalories = new AddCalories();
            addCalories.Owner = this;
            addCalories.calendario.DisplayDate = new DateTime(anioActual, (int)mesActual, 1);
            addCalories.ShowDialog();
            if (addCalories.DialogResult == true)
            {
                DateTime fecha = addCalories.fecha_;
                String comida = addCalories.comida_;
                double calorias = addCalories.calorias_;

                IEnumerable<Dia> resultsDia = Dias.Where(d => d.Fecha.Equals(fecha));
                if (!resultsDia.Any())
                {
                    Dia d = new Dia(fecha, new ObservableCollection<Comida>{ new Comida(comida, calorias) });
                    Dias.Add(d);
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
        void mesChart(Object sender, EventArgs e){ monthlyChart.DrawMonth(Dias.ToList(), (int)mesActual, anioActual); } 
        void sizeChanged(object sender,  EventArgs e)
        {
            if(monthlyChart.Visibility == Visibility.Visible) monthlyChart.DrawMonth(Dias.ToList(), (int)mesActual, anioActual);
            else if(dailyChart.Visibility == Visibility.Visible) dailyChart.DrawDay(diaActual.Comidas.ToList());
        }
        void diaChartChanged(object sender, EventArgs e)
        {
            monthlyChart.Visibility = Visibility.Hidden;
            dailyChart.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            diaActual = (Dia)listaCalorias.listaDia.SelectedItem;

            if (diaActual != null)
            {
                dailyChart.DrawDay(diaActual.Comidas.ToList());
            }
            else
            {
                monthlyChart.Visibility = Visibility.Visible;
                monthlyChart.DrawMonth(Dias.ToList(), (int)mesActual, anioActual);
                dailyChart.Visibility = Visibility.Hidden;
                backButton.Visibility = Visibility.Hidden;
            }
        }
        void diaChart(object sender, EventArgs e){ if (dailyChart.Visibility == Visibility.Visible) dailyChart.DrawDay(diaActual.Comidas.ToList());}
        void backToMonthly(object sender, RoutedEventArgs e)
        {
            monthlyChart.Visibility = Visibility.Visible;
            monthlyChart.DrawMonth(Dias.ToList(), (int)mesActual, anioActual);
            dailyChart.Visibility = Visibility.Hidden;
            backButton.Visibility = Visibility.Hidden;
        }

        private void mesAnterior(object sender, RoutedEventArgs e)
        {
           
            if(mesActual == Meses.Enero) 
            { 
                mesActual = Meses.Diciembre;
                year.Content = --anioActual;
            } 
            else { mesActual--; }
            month.Content = mesActual.ToString();
            monthlyChart.DrawMonth(Dias.ToList(), (int)mesActual, anioActual);
        }

        private void mesSiguiente(object sender, RoutedEventArgs e)
        {
            if (mesActual == Meses.Diciembre) 
            { 
                mesActual = Meses.Enero;
                year.Content = ++anioActual;
            } 
            else { mesActual++; }
            month.Content = mesActual.ToString();
            monthlyChart.DrawMonth(Dias.ToList(), (int)mesActual, anioActual);

        }

        private void saveCollectionOnClose(object sender, EventArgs e)
        {
            string jsonString = JsonSerializer.Serialize(Dias);
            File.WriteAllText(path, jsonString);
        }
    }
}

