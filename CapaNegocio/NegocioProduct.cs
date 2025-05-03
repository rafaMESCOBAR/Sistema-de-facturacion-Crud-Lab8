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

        // Método unificado para obtener productos con o sin filtro
        public List<Product> GetProducts(string filterText = null)
        {
            try
            {
                // Si el filtro está vacío, se obtienen todos los productos
                if (string.IsNullOrWhiteSpace(filterText))
                {
                    return datosProduct.FilterProducts(null);
                }
                // Si hay texto de filtro, se filtran los productos por nombre
                else
                {
                    return datosProduct.FilterProducts(filterText);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al obtener productos: " + ex.Message);
            }
        }

        // Métodos adicionales de negocio

        // Calcular el valor total del inventario
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

        // Obtener productos con bajo stock (menos de 10 unidades)
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