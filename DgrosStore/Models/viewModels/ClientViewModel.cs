using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DgrosStore.Models.viewModels
{
    public class ClientViewModel
    {
        public Client Client { get; set; }
        [Required(ErrorMessage ="Debe introducir un telefono")]
        [RegularExpression(@"^[+-]?\d+(\.\d+)?$",ErrorMessage ="el telefono no es un telefono valido")]
        public String Telephone { get; set; }

        public HttpPostedFileBase UploadedFile { get; set; }
    }
}