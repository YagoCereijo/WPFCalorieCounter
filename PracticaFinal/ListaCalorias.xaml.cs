using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PracticaFinal
{
    /// <summary>
    /// Lógica de interacción para ListaCalorias.xaml
    /// </summary>
    public partial class ListaCalorias : Window
    {
        public ObservableCollection<Dia> Dias { get; set; }
    
        public ListaCalorias(ObservableCollection<Dia> dias)
        {
            InitializeComponent();
            Dias = dias;
            listaDia.ItemsSource = Dias;
        }

        private void listaDiaSourceChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listaDia.SelectedItem != null) listaComida.ItemsSource = ((Dia)listaDia.SelectedItem).Comidas;
        }

        private void borrarDia(object sender, RoutedEventArgs e)
        {
            Dias.Remove((Dia)listaDia.SelectedItem);
        }

        private void borrarComida(object sender, RoutedEventArgs e)
        {
            ((Dia)listaDia.SelectedItem).Comidas.Remove((Comida)listaComida.SelectedItem);
        }
    }
}
