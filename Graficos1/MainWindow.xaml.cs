using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graficos1
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

        private void drawGraphics(object sender, RoutedEventArgs e)
        {
            int numpuntos;
            double ancho, alto;
            ancho = lienzo.ActualWidth;
            alto = lienzo.ActualHeight;

            numpuntos = (int)ancho;

            Polyline polilinea = new Polyline();

            polilinea.Stroke = Brushes.Blue;

            Point[] puntos = new Point[numpuntos];

            for (int i = 0; i < numpuntos; i++)
            {
                puntos[i].Y = -1 / alto * Math.Pow(i - numpuntos / 2, 2) + alto - 20;
                puntos[i].X = i;
            }

            polilinea.Points = new PointCollection(puntos);
            lienzo.Children.Add(polilinea);
        }
    }
}
