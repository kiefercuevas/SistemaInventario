using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models
{
    public class SalesProducs
    {
        public int ProductId { get; set; }
        public int SalesId { get; set; }

        //propiedades de navegacion productos y ventas
        public virtual Product Product { get; set; }
        public virtual Sales Sales { get; set; }

        public int Quantity { get; set; }
        public int Discount { get; set; }

    }
}