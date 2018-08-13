using System.Collections.Generic;

namespace DgrosStore.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Direcction { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }

        //collecion de telefonos
        public ICollection<Telephone> Telephones { get; set; }

        //collecion de ventas a clientes(facturas)
        public ICollection<Sales> Sales { get; set; }
    }
}