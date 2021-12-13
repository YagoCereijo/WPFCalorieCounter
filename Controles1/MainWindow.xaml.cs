using System.Collections.Generic;
using System.Windows;

namespace Controles1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Amigo> modelo;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Amigo miamigo = new Amigo(textbox1.Text, textbox2.Text, 21);
            listbox.Items.Add(miamigo);
            modelo.Add(miamigo);


        }

        private void list_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                label1.Content = listbox.SelectedItem;
            }
        }
    }
}
