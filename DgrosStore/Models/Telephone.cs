using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models
{
    public class Telephone
    {
        public int TelephoneId { get; set; }

        [Required(ErrorMessage ="El telefono no puede estar vacio")]
        [RegularExpression(@"\b8[024]9\-?\d{3}\-?\d{4}\b", ErrorMessage = "El telefono no es un telefono valido")]
        public string Number { get; set; }


        //propiedades navegacion de cliente
        public Person Person { get; set; }
        public int PersonId { get; set; }
    }
}