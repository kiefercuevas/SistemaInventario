using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
namespace DgrosStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public float ShoppingPrice { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public bool State { get; set; }
        public string Image { get; set; }
        public float SellingPrice { get; set; }
        public int MinimunStock { get; set; }

        //propiedades de navegacion Categoria
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        //propiedades de navegacion tienda
        public Store Store { get; set; }
        public int StoreId { get; set; }

        //colleccion de ventas
        public virtual ICollection<SalesProducs> SalesProducs { get; set; }

        //colleccion de compras
        public virtual ICollection<ShoppingProducts> ShoppingProducts { get; set; }
    }
}