using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DgrosStore.Models.customValidation;

namespace DgrosStore.Models
{
    public class Person
    {
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Debe introducir el nombre del cliente")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [MaxLength(20, ErrorMessage = "El nombre debe ser de 20 o menos caracteres")]
        public string Name { get; set; }

        
        public string Direcction { get; set; }

        [PersonEmailValidation]
        [Required(ErrorMessage = "Debe de introducir un Email")]
        [EmailAddress(ErrorMessage = "El email no es valido")]
        public string Email { get; set; }
        public string Image { get; set; }
        public bool State { get; set; }

        public string Type { get; set; }

        //collecion de telefonos
        public ICollection<Telephone> Telephones { get; set; }

    }
}