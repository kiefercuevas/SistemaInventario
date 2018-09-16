using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models.viewModels
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public float ShoppingPrice { get; set; }
        public float SellingPrice { get; set; }
        public int Stock { get; set; }
        
    }
}