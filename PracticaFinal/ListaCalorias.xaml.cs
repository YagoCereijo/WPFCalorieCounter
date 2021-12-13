using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

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
            foreach (Dia item in Dias)
            {
                item.Comidas.CollectionChanged += update;
            }
            Dias.CollectionChanged += update;
            listaDia.ItemsSource = Dias;
            
            
        }

        private void listaDiaSourceChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaDia.SelectedItem != null) listaComida.ItemsSource = ((Dia)listaDia.SelectedItem).Comidas;
            else listaComida.ItemsSource = null;
        }

        private void borrarDia(object sender, RoutedEventArgs e)
        {
            if (listaDia.SelectedItem == null) MessageBox.Show("Selecciona un día primero");
            else
            {
                Dias.Remove((Dia)listaDia.SelectedItem);
                listaComida.ItemsSource = null;
            }

        }

        private void borrarComida(object sender, RoutedEventArgs e)
        {
            if (listaComida.SelectedItem == null) MessageBox.Show("Selecciona una comida primero");
            else
            {
                Collection<Comida> comidas = ((Dia)listaDia.SelectedItem).Comidas;
                comidas.Remove((Comida)listaComida.SelectedItem);
                if (comidas.Count == 0) Dias.Remove((Dia)listaDia.SelectedItem);
            }
        }

        private void update(object sender, NotifyCollectionChangedEventArgs e)
        {
            listaDia.Items.Refresh();
        }

        void onDiasManageInnerChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Dia item in e.NewItems)
                {
                    item.Comidas.CollectionChanged += update;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Dia item in e.OldItems)
                {
                    item.Comidas.CollectionChanged -= update;
                }
            }
        }
    }
}
