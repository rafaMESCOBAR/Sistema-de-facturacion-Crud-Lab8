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
using CapaEntidad;
using CapaNegocio;

namespace SistemaFacturacion
{
    public partial class ProductPage : Page
    {
        private NegocioProduct negocioProduct = new NegocioProduct();

        public ProductPage()
        {
            InitializeComponent();
            // No cargar productos automáticamente
            // En lugar de eso, mostrar una lista vacía o un mensaje
            dgProducts.ItemsSource = new List<Product>(); // Lista vacía inicial
            UpdateStatistics();
        }

        private void LoadProducts(string filterText)
        {
            try
            {
                // Solo cargar productos si hay un filtro
                if (!string.IsNullOrWhiteSpace(filterText))
                {
                    List<Product> products = negocioProduct.GetProducts(filterText);
                    dgProducts.ItemsSource = products;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                // Mostrar el valor total del inventario (esto podría ser calculado sin cargar todos los productos)
                decimal totalValue = negocioProduct.CalculateTotalInventoryValue();
                txtTotalValue.Text = totalValue.ToString("C");

                // Mostrar la cantidad de productos con bajo stock
                List<Product> lowStockProducts = negocioProduct.GetLowStockProducts();
                txtLowStock.Text = lowStockProducts.Count.ToString() + " productos";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar estadísticas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filterText = txtFilter.Text.Trim();

                if (string.IsNullOrWhiteSpace(filterText))
                {
                    MessageBox.Show("Por favor ingrese un criterio de búsqueda", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                LoadProducts(filterText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtFilter.Text = "";
            // Limpiar la grilla en lugar de cargar todos los productos
            dgProducts.ItemsSource = new List<Product>();
        }
    }
}