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
    public class DatosInvoice
    {
        // Método para listar facturas
        public List<Invoice> ListInvoices()
        {
            List<Invoice> invoices = new List<Invoice>();

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM invoices WHERE active = 1", connection);
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Invoice invoice = new Invoice
                        {
                            InvoiceId = Convert.ToInt32(reader["invoice_id"]),
                            CustomerId = Convert.ToInt32(reader["customer_id"]),
                            Date = Convert.ToDateTime(reader["date"]),
                            Total = Convert.ToDecimal(reader["total"]),
                            Active = Convert.ToBoolean(reader["active"])
                        };

                        invoices.Add(invoice);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar facturas: " + ex.Message);
            }

            return invoices;
        }
    }
}
