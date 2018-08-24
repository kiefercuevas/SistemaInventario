using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models;
namespace DgrosStore.Models.customValidation
{
    public class IdCardValidation : ValidationAttribute
    {
        private readonly DgrosStoreContext dgrosStore;

        public IdCardValidation()
        {
            dgrosStore = new DgrosStoreContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var client = (Client)validationContext.ObjectInstance;
            var clientIDcard = client.IdCard.Replace("-","");
            var clientInDb = dgrosStore.Clients.SingleOrDefault(c => c.IdCard == clientIDcard);


            if (clientInDb != null)
            {
                if(clientInDb.ClientId != client.ClientId)
                {
                    return new ValidationResult("Ya existe un cliente con esta cedula");
                }
            }

                return ValidationResult.Success;
        }
    }
}