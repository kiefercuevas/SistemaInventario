using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DgrosStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Debe introducir un nombre para la categoria")]
        public string Name { get; set; }

        public bool State { get; set; }

        //colleccion de productos
        public ICollection<Product> Products {get; set;}
    }
}