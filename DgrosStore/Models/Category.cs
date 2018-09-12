using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DgrosStore.Models.customValidation;
namespace DgrosStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Debe introducir un nombre para la categoria")]
        [CategoryNameValidation]
        public string Name { get; set; }

        public bool State { get; set; }

        //colleccion de productos
        public ICollection<Product> Products {get; set;}
    }
}