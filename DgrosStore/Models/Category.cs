using System.Collections.Generic;

namespace DgrosStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        //colleccion de productos
        public ICollection<Product> Products {get; set;}
    }
}