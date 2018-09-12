using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models;
using DgrosStore.Models.viewModels;
using System.Data.Entity;

namespace DgrosStore.Models.customValidation
{
    public class ClientTelephoneValidation :ValidationAttribute
    {
        private readonly DgrosStoreContext dgrosStore;

        public ClientTelephoneValidation()
        {
            dgrosStore = new DgrosStoreContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var client = (ClientViewModel)validationContext.ObjectInstance;
            var clientTelephone = client.Telephone;

            var clientInDb = dgrosStore.Clients
                                    .Join(dgrosStore.Telephones,
                                          c => c.ClientId,
                                          t => t.ClientId,
                                          (c, t) => new { Client = c, Telephone = t })
                                    .Where(c => c.Telephone.Number == clientTelephone)
                                    .SingleOrDefault();


            if (clientInDb != null && clientInDb.Client.ClientId != client.Client.ClientId)
                return new ValidationResult("Ya existe un cliente con ese numero telefonico");

            return ValidationResult.Success;
        }
    }
}