using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models.customValidation;

namespace DgrosStore.Models.viewModels
{
    public class ProductViewModelDTO
    {
        public Product Product { get; set; }

        [UploadedProductImageValidation]
        public HttpPostedFileBase UploadedFile { get; set; }
        public IEnumerable<Store> Stores { get; set; }
        public IEnumerable<Category> Categories { get; set; }
 
        public IEnumerable<Provider> Providers { get; set; }
        public IEnumerable<Discount> Discounts { get; set; }
    }
}