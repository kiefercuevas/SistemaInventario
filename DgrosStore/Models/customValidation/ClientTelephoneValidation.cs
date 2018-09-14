using System.Linq;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models.viewModels;

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
                                          t => t.PersonId,
                                          (c, t) => new { Client = c, Telephone = t })
                                    .Where(c => c.Telephone.Number == clientTelephone)
                                    .SingleOrDefault();

            

            if (clientInDb != null && clientInDb.Client.ClientId != client.Client.ClientId)
                return new ValidationResult("Ya existe un cliente con ese numero telefonico");
            /*else
            {
                var providerInDb = dgrosStore.Providers.SingleOrDefault(p => p. == clientTelephone);
                if(providerInDb != null)
                    return new ValidationResult("El numero introducido pertenece a un proveedor registrado");
                else
                {
                    var storeInDb = dgrosStore.Stores.SingleOrDefault(s => s.Telephone == clientTelephone);
                    if(storeInDb != null)
                        return new ValidationResult("El numero introducido pertenece a una de las sucursales de la empresa");
                }
            }*/
            return ValidationResult.Success;
        }
    }
}