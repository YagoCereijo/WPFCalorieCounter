using System.Windows;
using System.Windows.Media;

namespace parctica1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            et1.Content = "Cambio otra vez";
            et2.Foreground = Brushes.Yellow;
        }
    }
}
