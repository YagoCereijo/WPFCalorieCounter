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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        ObservableCollection<Comida> comidas;


        public MainWindow()
        {
            InitializeComponent();
            comidas = new ObservableCollection<Comida>();
            comidas.CollectionChanged += this.onComidasChange;
            monthlychart.I
        }

        public void ShowAddCalories(object sender, RoutedEventArgs e)
        {
            
            AddCalories addCalories = new AddCalories();
            addCalories.Owner = this;
            addCalories.ShowDialog();
            if (addCalories.DialogResult == true)
            {
                Comida nuevaComida = addCalories.comidaInput;
                IEnumerable<Comida> query = comidas.Where(c => c.fecha.Equals(nuevaComida.fecha) && c.comida.Equals(nuevaComida.comida));
                if (!query.Any()) comidas.Add(nuevaComida);
                else MessageBox.Show("Ya existe esa comida");
            }
        }

        void onComidasChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            monthlychart.addData(comidas);
        }
    }
}
