using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

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

            loadData();

            Dias.CollectionChanged += mesChart;
            Dias.CollectionChanged += onDiasManageInnerChange;
            monthlyChart.SizeChanged += sizeChanged;
            monthlyChart.Loaded += mesChart;
            dailyChart.SizeChanged += sizeChanged;
        }

        // LOAD DATA
        void loadData()
        {
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                Dias = JsonSerializer.Deserialize<ObservableCollection<Dia>>(jsonString);
                foreach (Dia item in Dias)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += mesChart;
                    item.Comidas.CollectionChanged += mesChart;
                    item.Comidas.CollectionChanged += diaChart;
                }
            }
            else
            {
                Dias = new ObservableCollection<Dia>();
            }
        }

        // BUTTON EVENTS
        public void ShowDayList(object sender, RoutedEventArgs e)
        {
            listaCalorias = new ListaCalorias(Dias);
            listaCalorias.Owner = this;
            listaCalorias.listaDia.SelectionChanged += diaChart;
            listaCalorias.botonBorrarComida.Click += diaChart;
            listaCalorias.Closed += backToMonthly;
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
                int calorias = addCalories.calorias_;


                IEnumerable<Dia> resultsDia = Dias.Where(d => d.Fecha.Equals(fecha));
                if (!resultsDia.Any())
                {
                    Dia d = new Dia(fecha, new ObservableCollection<Comida> { new Comida(comida, calorias) });
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
                        else d.Comidas.Add(new Comida(comida, calorias));

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
                    item.Comidas.CollectionChanged += mesChart;
                    item.Comidas.CollectionChanged += diaChartInner;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Dia item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= mesChart;
                    item.Comidas.CollectionChanged -= diaChartInner;
                    item.Comidas.CollectionChanged -= mesChart;
                }
            }
        }
        void mesChart(Object sender, EventArgs e) { monthlyChart.DrawMonth(Dias.ToList(), (int)mesActual, anioActual); }
        void sizeChanged(object sender, EventArgs e)
        {
            if (monthlyChart.Visibility == Visibility.Visible) monthlyChart.DrawMonth(Dias.ToList(), (int)mesActual, anioActual);
            else if (dailyChart.Visibility == Visibility.Visible) dailyChart.DrawDay(diaActual.Comidas.ToList());
        }
        void diaChart(object sender, EventArgs e)
        {
            monthlyChart.Visibility = Visibility.Collapsed;
            dailyChart.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            month.Visibility = Visibility.Collapsed;
            year.Visibility = Visibility.Collapsed;
            previousButton.Visibility = Visibility.Collapsed;
            nextButton.Visibility = Visibility.Collapsed;
            date.Visibility = Visibility.Visible;

            diaActual = (Dia)listaCalorias.listaDia.SelectedItem;
            

            if (diaActual != null)
            {
                date.Content = diaActual.Fecha;
                dailyChart.DrawDay(diaActual.Comidas.ToList());
            }
            else
            {
                backToMonthly(null, null);
            }

        }

        void diaChartInner(object sender, EventArgs e) { if (dailyChart.Visibility == Visibility.Visible) dailyChart.DrawDay(diaActual.Comidas.ToList()); }



        void backToMonthly(object sender, EventArgs e)
        {
            monthlyChart.Visibility = Visibility.Visible;
            monthlyChart.DrawMonth(Dias.ToList(), (int)mesActual, anioActual);
            dailyChart.Visibility = Visibility.Hidden;
            backButton.Visibility = Visibility.Hidden;
            date.Visibility = Visibility.Hidden;
            month.Visibility = Visibility.Visible;
            year.Visibility = Visibility.Visible;
            previousButton.Visibility = Visibility.Visible;
            nextButton.Visibility = Visibility.Visible;
        }

        private void mesAnterior(object sender, RoutedEventArgs e)
        {

            if (mesActual == Meses.Enero)
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