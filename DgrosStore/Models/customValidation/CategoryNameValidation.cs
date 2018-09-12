using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models;

namespace DgrosStore.Models.customValidation
{
    public class CategoryNameValidation :ValidationAttribute
    {
            private readonly DgrosStoreContext dgrosStore;

            public CategoryNameValidation()
            {
                dgrosStore = new DgrosStoreContext();
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var Category = (Category)validationContext.ObjectInstance;
                var categoryName = Category.Name.ToLower();
                var categoryInDb = dgrosStore.Categories.SingleOrDefault(p => p.Name.ToLower() == categoryName);


                if (categoryInDb != null)
                {
                    if (categoryInDb.CategoryId != Category.CategoryId)
                    {
                        return new ValidationResult("Ya existe una categoria con ese nombre");
                    }
                }
                return ValidationResult.Success;
            }
    }
}