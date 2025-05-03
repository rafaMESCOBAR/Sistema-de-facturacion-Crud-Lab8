using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos; // Añade esta línea

namespace CapaNegocio
{
    public class NegocioCustomer
    {
        private DatosCustomer datosCustomer = new DatosCustomer();

        // Método para listar todos los clientes
        public List<Customer> ListCustomers()
        {
            try
            {
                return datosCustomer.ListCustomers();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa de negocio al listar clientes: " + ex.Message);
            }
        }
    }
}