using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models
{
    public class Provider :Person
    {
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "El campo rnc es obligatorio")]
        [RegularExpression(@"\b[145]\d{2}\-?\d{5}\-?\d{1}\b", ErrorMessage = "El RNC no es valido")]
        public string Rnc { get; set; }
        //collecion de compras a Proveedores
        public ICollection<Shopping> Shoppings { get; set; }

        //colleccion de productos
        public ICollection<Product> Products { get; set; }
    }
}