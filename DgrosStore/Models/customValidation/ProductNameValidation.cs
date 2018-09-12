using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models;

namespace DgrosStore.Models.customValidation
{
    public class ProductNameValidation : ValidationAttribute
    {
        private readonly DgrosStoreContext dgrosStore;

        public ProductNameValidation()
        {
            dgrosStore = new DgrosStoreContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Product = (Product)validationContext.ObjectInstance;
            var productName = Product.Name.ToLower();
            var productInDb = dgrosStore.Products.SingleOrDefault(p => p.Name.ToLower() == productName);


            if (productInDb != null)
            {
                if (productInDb.ProductId != Product.ProductId)
                {
                    return new ValidationResult("Ya existe un producto con ese nombre");
                }
            }
            return ValidationResult.Success;
        }
    }
}