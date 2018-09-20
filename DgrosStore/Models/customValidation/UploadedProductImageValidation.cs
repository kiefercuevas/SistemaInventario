using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models.viewModels;
using System.Configuration;
using System.Web.Configuration;

namespace DgrosStore.Models.customValidation
{
    public class UploadedProductImageValidation : ValidationAttribute
    {
        private readonly HttpRuntimeSection Section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] AdmitedImageType = { "image/jpeg", "image/png" };
            const int MAX_SIZE = 2;
            

            ProductViewModelDTO product = (ProductViewModelDTO)validationContext.ObjectInstance;
            float bytes = 1024;
            float megabyte = 0;


            if (product.UploadedFile == null)
                return ValidationResult.Success;

            megabyte = (product.UploadedFile.ContentLength / bytes) / bytes;

            if(!AdmitedImageType.Contains(product.UploadedFile.ContentType))
                return new ValidationResult("Solo puede introducir imagenes con formato JPG y PNG");
            if (megabyte > MAX_SIZE || megabyte > Section.MaxRequestLength)
                return new ValidationResult("El archivo no puede exceder los 2mb");
            

            return ValidationResult.Success;
        }

    }
}