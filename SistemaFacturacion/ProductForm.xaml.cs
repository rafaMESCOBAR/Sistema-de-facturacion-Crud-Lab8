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
using System.Windows.Shapes;
using CapaEntidad;
using CapaNegocio;

namespace SistemaFacturacion
{
    public partial class ProductForm : Window
    {
        private NegocioProduct negocioProduct = new NegocioProduct();
        private Product currentProduct;
        private bool isNew = true;

        // Constructor para nuevo producto
        public ProductForm()
        {
            InitializeComponent();
            currentProduct = new Product();
            isNew = true;
            this.Title = "Agregar Producto";
        }

        // Constructor para editar producto existente
        public ProductForm(int productId)
        {
            InitializeComponent();
            isNew = false;
            this.Title = "Editar Producto";
            LoadProduct(productId);
        }

        private void LoadProduct(int productId)
        {
            try
            {
                currentProduct = negocioProduct.GetProductById(productId);

                if (currentProduct != null)
                {
                    txtProductId.Text = currentProduct.ProductId.ToString();
                    txtName.Text = currentProduct.Name;
                    txtPrice.Text = currentProduct.Price.ToString();
                    txtStock.Text = currentProduct.Stock.ToString();
                }
                else
                {
                    MessageBox.Show("Producto no encontrado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar campos
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("El nombre del producto es obligatorio", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtName.Focus();
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Ingrese un precio válido mayor que cero", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPrice.Focus();
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("Ingrese un stock válido no negativo", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtStock.Focus();
                    return;
                }

                // Actualizar objeto producto
                currentProduct.Name = txtName.Text.Trim();
                currentProduct.Price = price;
                currentProduct.Stock = stock;
                currentProduct.Active = true;

                bool success;

                if (isNew)
                {
                    // Crear nuevo producto
                    int productId = negocioProduct.CreateProduct(currentProduct);
                    success = productId > 0;
                    if (success)
                    {
                        MessageBox.Show("Producto agregado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    // Actualizar producto existente
                    success = negocioProduct.UpdateProduct(currentProduct);
                    if (success)
                    {
                        MessageBox.Show("Producto actualizado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                if (success)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
