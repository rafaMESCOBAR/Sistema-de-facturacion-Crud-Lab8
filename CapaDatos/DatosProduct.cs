using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class DatosProduct
    {
        // Método unificado para filtrar productos (o listar todos si no hay filtro)
        public List<Product> FilterProducts(string filterText = null)
        {
            List<Product> products = new List<Product>();

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("USP_FilterProducts", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetro opcional para filtrar
                    if (filterText != null)
                    {
                        command.Parameters.AddWithValue("@FilterText", filterText);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@FilterText", DBNull.Value);
                    }

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            Name = reader["name"].ToString(),
                            Price = Convert.ToDecimal(reader["price"]),
                            Stock = Convert.ToInt32(reader["stock"]),
                            Active = Convert.ToBoolean(reader["active"])
                        };

                        products.Add(product);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar productos: " + ex.Message);
            }

            return products;
        }
    }
}