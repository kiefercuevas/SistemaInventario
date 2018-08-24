using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models.customValidation;
namespace DgrosStore.Models
{
    public class Client
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage ="Debe introducir el nombre del cliente")]
        [MinLength(3,ErrorMessage ="El nombre debe tener al menos 3 caracteres")]
        [MaxLength(20,ErrorMessage ="El nombre debe ser de 20 o menos caracteres")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Debe introducir el apellido del cliente")]
        [MinLength(3, ErrorMessage = "El apellido debe tener al menos 3 caracteres")]
        [MaxLength(20, ErrorMessage = "El apellido debe ser de 20 o menos caracteres")]
        public string LastName { get; set; }
        public string Direcction { get; set; }
        [EmailAddress(ErrorMessage ="El email no es valido")]
        public string Email { get; set; }
        public string Image { get; set; }

        [IdCardValidation]
        [Required(ErrorMessage ="Debe introducir una cedula valida")]
        [RegularExpression(@"\b\d{3}\-?\d{7}\-?\d{1}\b", ErrorMessage ="la cedula no es valida")]
        public string IdCard { get; set; }

        //collecion de telefonos
        public ICollection<Telephone> Telephones { get; set; }

        //collecion de ventas a clientes(facturas)
        public ICollection<Sales> Sales { get; set; }
    }
}