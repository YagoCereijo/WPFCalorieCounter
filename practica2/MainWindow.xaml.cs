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

namespace practica2
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key){
                case Key.Left:
                    if (Grid.GetColumn(Rectagulo) > 0)
                        Grid.SetColumn(Rectagulo, Grid.GetColumn(Rectagulo) - 1);
                    break;
                case Key.Right:
                    if(Grid.GetColumn(Rectagulo) < Grid.ColumnDefinitions.Count)
                        Grid.SetColumn(Rectagulo, Grid.GetColumn(Rectagulo) + 1);
                    break;
                case Key.Up:
                    if(Grid.GetRow(Rectagulo) > 0)
                        Grid.SetRow(Rectagulo, Grid.GetRow(Rectagulo) - 1);
                    break;
                case Key.Down:
                    if(Grid.GetRow(Rectagulo) < Grid.RowDefinitions.Count)
                        Grid.SetRow(Rectagulo, Grid.GetRow(Rectagulo) + 1);
                    break;
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
      

            RotateTransform rt = (RotateTransform)Rectagulo.RenderTransform;
            rt.Angle += 5;
        }
    }
}
