using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para AddCalories.xaml
    /// </summary>
    public partial class AddCalories : Window
    {
        public DateTime fecha_;
        public String comida_;
        public double calorias_;
        
        public AddCalories()
        {
            InitializeComponent();
        }

        private void addCalories(object sender, RoutedEventArgs e)
        {
            fecha_ = calendario.SelectedDate.Value;
            comida_ = comida.Text;
            calorias_ = calorias.Value;
            DialogResult = true;
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {

            DialogResult = false;
        }
    }
}
