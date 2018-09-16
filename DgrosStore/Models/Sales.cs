using System;
using System.Collections.Generic;
namespace DgrosStore.Models
{
    public class Sales :Transaction
    {
        
        //propiedades de navegacion tienda
        public Store Store{ get; set; }
        public int StoreId { get; set; }

        public float Total { get; set; }
        //propiedades de navegacion cliente
        public virtual ICollection<Client> Clients { get; set; }

        //colleccion de ventas
        public virtual ICollection<SalesProducs> SalesProducs { get; set; }

        public virtual ICollection<Discount> Discounts { get; set; }

    }
}