using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class DatosProduct
    {
        // Método para filtrar productos (READ)
        public List<Product> FilterProducts(string filterText = null)
        {
            List<Product> products = new List<Product>();

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("USP_FilterProducts", connection);
                    command.CommandType = CommandType.StoredProcedure;

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

        // Método para obtener un producto por ID (READ individual)
        public Product GetProductById(int productId)
        {
            Product product = null;

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM products WHERE product_id = @ProductId", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@ProductId", productId);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        product = new Product
                        {
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            Name = reader["name"].ToString(),
                            Price = Convert.ToDecimal(reader["price"]),
                            Stock = Convert.ToInt32(reader["stock"]),
                            Active = Convert.ToBoolean(reader["active"])
                        };
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto: " + ex.Message);
            }

            return product;
        }

        // Método para insertar un nuevo producto (CREATE)
        public int InsertProduct(Product product)
        {
            int productId = 0;

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("INSERT INTO products (name, price, stock, active) " +
                                                       "OUTPUT INSERTED.product_id " +
                                                       "VALUES (@Name, @Price, @Stock, @Active)", connection);

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Stock", product.Stock);
                    command.Parameters.AddWithValue("@Active", product.Active);

                    connection.Open();

                    productId = (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el producto: " + ex.Message);
            }

            return productId;
        }

        // Método para actualizar un producto existente (UPDATE)
        public bool UpdateProduct(Product product)
        {
            bool success = false;

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("UPDATE products " +
                                                       "SET name = @Name, price = @Price, stock = @Stock, active = @Active " +
                                                       "WHERE product_id = @ProductId", connection);

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@ProductId", product.ProductId);
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Stock", product.Stock);
                    command.Parameters.AddWithValue("@Active", product.Active);

                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();
                    success = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el producto: " + ex.Message);
            }

            return success;
        }

        // Método para eliminar lógicamente un producto (DELETE - cambiar estado a inactivo)
        public bool DeleteProduct(int productId)
        {
            bool success = false;

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("UPDATE products SET active = 0 WHERE product_id = @ProductId", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@ProductId", productId);

                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();
                    success = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el producto: " + ex.Message);
            }

            return success;
        }
    }
}