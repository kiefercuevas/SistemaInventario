using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models
{
    public class ShoppingProducts
    {
        public int ProductId { get; set; }
        public int ShoppingId { get; set; }
        
        //propiedades de navegacion productos y ventas
        public virtual Product Product { get; set; }
        public virtual Shopping Shopping { get; set; }

        public int Quantity { get; set; }
        public int Discount { get; set; }
    }
}