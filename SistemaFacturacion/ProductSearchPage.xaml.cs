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
        private Product selectedProduct;

        public ProductPage()
        {
            InitializeComponent();
            UpdateStatistics();
        }

        private void LoadProducts(string filterText = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filterText))
                {
                    dgProducts.ItemsSource = new List<Product>();
                }
                else
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
                decimal totalValue = negocioProduct.CalculateTotalInventoryValue();
                txtTotalValue.Text = totalValue.ToString("C");

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
            dgProducts.ItemsSource = new List<Product>();
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProduct = dgProducts.SelectedItem as Product;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ProductForm productForm = new ProductForm();
            bool? result = productForm.ShowDialog();

            if (result == true)
            {
                // Si el filtro tiene texto, recargar la búsqueda
                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                {
                    LoadProducts(txtFilter.Text);
                }

                UpdateStatistics();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Product product = btn.DataContext as Product;
                if (product != null)
                {
                    ProductForm productForm = new ProductForm(product.ProductId);
                    bool? result = productForm.ShowDialog();

                    if (result == true)
                    {
                        // Si el filtro tiene texto, recargar la búsqueda
                        if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                        {
                            LoadProducts(txtFilter.Text);
                        }

                        UpdateStatistics();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn != null)
                {
                    Product product = btn.DataContext as Product;
                    if (product != null)
                    {
                        MessageBoxResult messageResult = MessageBox.Show(
                            $"¿Está seguro que desea eliminar el producto '{product.Name}'?",
                            "Confirmar eliminación",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (messageResult == MessageBoxResult.Yes)
                        {
                            bool success = negocioProduct.DeleteProduct(product.ProductId);

                            if (success)
                            {
                                MessageBox.Show("Producto eliminado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                                // Si el filtro tiene texto, recargar la búsqueda
                                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                                {
                                    LoadProducts(txtFilter.Text);
                                }

                                UpdateStatistics();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}