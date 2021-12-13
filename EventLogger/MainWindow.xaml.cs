using System;
using System.Windows;
using System.Windows.Controls;

namespace EventLogger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UIElement[] elementos = { this, grid, boton, cabecera, panel, textoboton };
            foreach (UIElement i in elementos)
            {
                i.PreviewKeyDown += gestorglobal;
                i.PreviewKeyUp += gestorglobal;
                i.PreviewTextInput += gestorglobal;
                i.KeyDown += gestorglobal;
                i.KeyUp += gestorglobal;
                i.TextInput += gestorglobal;
                i.PreviewMouseDown += gestorglobal;
                i.PreviewMouseUp += gestorglobal;
                i.MouseDown += gestorglobal;
                i.MouseUp += gestorglobal;
            }
            boton.Click += gestorglobal;
            cabecera.Text = string.Format(strFormat, "Evento", "Sender", "Fuente", "Fuente Original");

        }

        string strFormat = "{0,-30} {1,-15} {2,-15} {3,-15}";

        void gestorglobal(Object sender, RoutedEventArgs e)
        {
            TextBlock linea = new TextBlock();
            linea.Text = String.Format(strFormat,
            e.RoutedEvent.Name,
            nombreobjeto(sender),
            nombreobjeto(e.Source),
            nombreobjeto(e.OriginalSource));
            panel.Children.Add(linea);
            scroll.ScrollToBottom();
        }
        string nombreobjeto(Object obj)
        {
            string[] parseada = obj.GetType().ToString().Split('.');
            return parseada[parseada.Length - 1];
        }
    }
}
