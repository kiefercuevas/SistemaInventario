using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models
{
    public class Telephone
    {
        public int TelephoneId { get; set; }

        [Required(ErrorMessage ="El telefono no puede estar vacio")]
        public string Number { get; set; }


        //propiedades navegacion de cliente
        public Client Client { get; set; }
        public int ClientId { get; set; }
    }
}