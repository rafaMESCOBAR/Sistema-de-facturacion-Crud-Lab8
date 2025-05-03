using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SistemaFacturacion
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Cargar la página de productos por defecto
            MainFrame.Navigate(new ProductPage());
        }

        private void MenuItemSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemProductos_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductPage());
        }
    }
}