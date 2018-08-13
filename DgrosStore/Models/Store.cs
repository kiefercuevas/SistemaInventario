using System.Collections.Generic;
namespace DgrosStore.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Address{ get; set; }
        public string Telephone { get; set; }
        public string Logo { get; set; }
        public string Slogan { get; set; }


        //collecion de ventas de la tienda(facturas)
        public ICollection<Sales> Sales { get; set; }


        //collecion de compras de la tienda
        public ICollection<Shopping> Shoppings { get; set; }

        //colleccion de productos
        public ICollection<Product> Products { get; set; }

    }
}