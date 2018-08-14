using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models.customValidation
{
    public class SellingPriceValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var product = (Product) validationContext.ObjectInstance;

            if(product.SellingPrice <= 0)
            {
                return new ValidationResult("El precio de compra debe ser mayor de 0");
            }

            return ValidationResult.Success;
        }
    }
}