using System;
using System.ComponentModel.DataAnnotations;

namespace DgrosStore.Models.viewModels
{
    public class GetClientModel
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string Email { get; set; }

        [RegularExpression(@"\b8[024]9\-?\d{3}\-?\d{4}\b", ErrorMessage = "El telefono no es un telefono valido")]
        public String Telephone { get; set; }
    }
}