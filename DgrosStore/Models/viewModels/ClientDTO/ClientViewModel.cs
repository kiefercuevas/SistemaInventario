using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models.customValidation;

namespace DgrosStore.Models.viewModels
{
    public class ClientViewModel
    {
        
        public Client Client { get; set; }

        [ClientTelephoneValidation]
        [RegularExpression(@"\b8[024]9\-?\d{3}\-?\d{4}\b", ErrorMessage ="El telefono no es un telefono valido")]
        public String Telephone { get; set; }

        public HttpPostedFileBase UploadedFile { get; set; }
    }
}