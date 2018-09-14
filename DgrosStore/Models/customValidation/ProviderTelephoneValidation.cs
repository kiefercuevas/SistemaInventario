using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models.viewModels;


namespace DgrosStore.Models.customValidation
{
    public class ProviderTelephoneValidation :ValidationAttribute
    {
        private readonly DgrosStoreContext dgrosStore;
        public ProviderTelephoneValidation()
        {
            dgrosStore = new DgrosStoreContext();
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var provider = (Provider)validationContext.ObjectInstance;

            var providerInDb = dgrosStore.Providers.SingleOrDefault(p => p.Telephone == provider.Telephone);


            if (providerInDb != null && providerInDb.ProviderId != provider.ProviderId)
                return new ValidationResult("Ya existe un proveedor con ese numero telefonico");
            else
            {
                var clientInDb = dgrosStore.Clients
                                    .Join(dgrosStore.Telephones,
                                          c => c.ClientId,
                                          t => t.ClientId,
                                          (c, t) => new { Client = c, Telephone = t })
                                    .Where(c => c.Telephone.Number == provider.Telephone)
                                    .SingleOrDefault();

                if (clientInDb != null)
                    return new ValidationResult("El numero introducido pertenece a un cliente registrado");
                else
                {
                    var storeInDb = dgrosStore.Stores.SingleOrDefault(s => s.Telephone == provider.Telephone);
                    if (storeInDb != null)
                        return new ValidationResult("El numero introducido pertenece a una de las sucursales de la empresa");
                }
            }
            return ValidationResult.Success;
        }

    }
}