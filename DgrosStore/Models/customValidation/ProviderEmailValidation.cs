using System.Linq;
using System.ComponentModel.DataAnnotations;


namespace DgrosStore.Models.customValidation
{

    public class ProviderEmailValidation :ValidationAttribute
    {
        private readonly DgrosStoreContext dgrosStore;
        public ProviderEmailValidation()
        {
            dgrosStore = new DgrosStoreContext();
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var provider = (Provider)validationContext.ObjectInstance;


            var providerInDb = dgrosStore.Providers
                .SingleOrDefault(p => p.Email.ToLower() == provider.Email.ToLower());


            if (providerInDb != null && providerInDb.ProviderId != provider.ProviderId)
                return new ValidationResult("Ya existe un proveedor con este Correo");
            else
            {
                var client = dgrosStore.Clients.SingleOrDefault(c => c.Email.ToLower() == provider.Email.ToLower());

                if (client != null)
                    return new ValidationResult("El Correo introducido pertenece a un cliente");
            }

            return ValidationResult.Success;
        }
    }
}