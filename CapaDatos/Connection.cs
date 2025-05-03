using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class Connection
    {
        // Cadena de conexión a la base de datos
        private static readonly string connectionString = "Data Source=DESKTOP-83DVELD\\SQLEXPRESS;" +
            "Initial Catalog=Lab7;" +
            "User ID=UserPrueba;" +
            "Password=123456;" +
            "TrustServerCertificate=True";

        // Método para obtener la conexión
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}