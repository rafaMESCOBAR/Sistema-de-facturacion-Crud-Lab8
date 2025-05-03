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
    public class DatosCustomer
    {
        // Método para listar todos los clientes
        public List<Customer> ListCustomers()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM customers WHERE active = 1", connection);
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            CustomerId = Convert.ToInt32(reader["customer_id"]),
                            Name = reader["name"].ToString(),
                            Address = reader["address"].ToString(),
                            Phone = reader["phone"].ToString(),
                            Active = Convert.ToBoolean(reader["active"])
                        };

                        customers.Add(customer);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar clientes: " + ex.Message);
            }

            return customers;
        }
    }
}