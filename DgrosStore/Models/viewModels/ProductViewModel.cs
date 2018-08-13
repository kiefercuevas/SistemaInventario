using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models.viewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public HttpPostedFileBase UploadedFile { get; set; }
        public IEnumerable<Store> Stores { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}