using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos; // Añade esta línea
namespace CapaNegocio
{
    public class NegocioProduct
    {
        private DatosProduct datosProduct = new DatosProduct();

        // Método para obtener productos filtrados (READ)
        public List<Product> GetProducts(string filterText = null)
        {
            try
            {
                return datosProduct.FilterProducts(filterText);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al obtener productos: " + ex.Message);
            }
        }

        // Método para obtener un producto por ID (READ individual)
        public Product GetProductById(int productId)
        {
            try
            {
                return datosProduct.GetProductById(productId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al obtener el producto: " + ex.Message);
            }
        }

        // Método para crear un nuevo producto (CREATE)
        public int CreateProduct(Product product)
        {
            try
            {
                // Validaciones de negocio
                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    throw new Exception("El nombre del producto es obligatorio");
                }

                if (product.Price <= 0)
                {
                    throw new Exception("El precio debe ser mayor que cero");
                }

                if (product.Stock < 0)
                {
                    throw new Exception("El stock no puede ser negativo");
                }

                // Establecer valor predeterminado
                product.Active = true;

                return datosProduct.InsertProduct(product);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al crear el producto: " + ex.Message);
            }
        }

        // Método para actualizar un producto existente (UPDATE)
        public bool UpdateProduct(Product product)
        {
            try
            {
                // Validaciones de negocio
                if (product.ProductId <= 0)
                {
                    throw new Exception("ID de producto no válido");
                }

                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    throw new Exception("El nombre del producto es obligatorio");
                }

                if (product.Price <= 0)
                {
                    throw new Exception("El precio debe ser mayor que cero");
                }

                if (product.Stock < 0)
                {
                    throw new Exception("El stock no puede ser negativo");
                }

                return datosProduct.UpdateProduct(product);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al actualizar el producto: " + ex.Message);
            }
        }

        // Método para eliminar un producto (DELETE)
        public bool DeleteProduct(int productId)
        {
            try
            {
                if (productId <= 0)
                {
                    throw new Exception("ID de producto no válido");
                }

                return datosProduct.DeleteProduct(productId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al eliminar el producto: " + ex.Message);
            }
        }

        // Métodos adicionales
        public decimal CalculateTotalInventoryValue()
        {
            decimal totalValue = 0;

            try
            {
                List<Product> products = datosProduct.FilterProducts();

                foreach (Product product in products)
                {
                    totalValue += product.Price * product.Stock;
                }

                return totalValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular el valor total del inventario: " + ex.Message);
            }
        }

        public List<Product> GetLowStockProducts()
        {
            try
            {
                List<Product> allProducts = datosProduct.FilterProducts();
                return allProducts.FindAll(p => p.Stock < 10);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener productos con bajo stock: " + ex.Message);
            }
        }
    }
}