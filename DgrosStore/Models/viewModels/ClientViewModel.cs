using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models.viewModels
{
    public class ClientViewModel
    {
        public Client Client { get; set; }
        public String Telephone { get; set; }

        public HttpPostedFileBase UploadedFile { get; set; }
    }
}