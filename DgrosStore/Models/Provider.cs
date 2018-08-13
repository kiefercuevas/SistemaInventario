using System.Collections.Generic;

namespace DgrosStore.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Direcction { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        //collecion de compras a Proveedores
        public ICollection<Shopping> Shoppings { get; set; }
    }
}