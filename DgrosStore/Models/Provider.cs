using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models.customValidation;
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

        [ProviderEmailValidation]
        [EmailAddress(ErrorMessage = "El email no es valido")]
        public string Email { get; set; }

        [ProviderTelephoneValidation]
        [Required(ErrorMessage = "Debe introducir el telefono del proveedor")]
        [RegularExpression(@"\b8[024]9\-?\d{3}\-?\d{4}\b", ErrorMessage = "El telefono no es un telefono valido")]
        public string Telephone { get; set; }

        public bool State { get; set; }

        //collecion de compras a Proveedores
        public ICollection<Shopping> Shoppings { get; set; }
    }
}