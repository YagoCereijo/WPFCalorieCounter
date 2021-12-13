using System;
using System.Windows;

namespace PracticaFinal
{
    /// <summary>
    /// Lógica de interacción para AddCalories.xaml
    /// </summary>
    public partial class AddCalories : Window
    {
        public DateTime fecha_;
        public String comida_;
        public int calorias_;

        public AddCalories()
        {
            InitializeComponent();
        }

        private void addCalories(object sender, RoutedEventArgs e)
        {
            if (comida.SelectedItem == null) MessageBox.Show("Selecciona una comida");
            else if (calendario.SelectedDate == null) MessageBox.Show("Selecciona una fecha");
            else
            {
                fecha_ = calendario.SelectedDate.Value;
                comida_ = comida.Text;
                calorias_ = (int)calorias.Value;
                DialogResult = true;
            }
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
