using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "Debe introducir el nombre del proveedor")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [MaxLength(20, ErrorMessage = "El nombre debe ser de 20 o menos caracteres")]
        public string Name { get; set; }

        public string Direcction { get; set; }

        [EmailAddress(ErrorMessage = "El email no es valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe introducir el telefono del proveedor")]
        public string Telephone { get; set; }

        //collecion de compras a Proveedores
        public ICollection<Shopping> Shoppings { get; set; }
    }
}