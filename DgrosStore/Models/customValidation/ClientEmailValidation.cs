using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DgrosStore.Models;
using DgrosStore.Models.viewModels;
namespace DgrosStore.Models.customValidation
{
    public class ClientEmailValidation : ValidationAttribute
    {
        private readonly DgrosStoreContext dgrosStore;

    public ClientEmailValidation()
    {
        dgrosStore = new DgrosStoreContext();
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var client = (Client)validationContext.ObjectInstance;
        var clientEmail = client.Email;
        

        if (clientEmail == null)
             return ValidationResult.Success;

            var clientInDb = dgrosStore.Clients
                .SingleOrDefault(c => c.Email.ToLower() == clientEmail.ToLower());


            if (clientInDb != null && clientInDb.ClientId != client.ClientId)
                return new ValidationResult("Ya existe un cliente con este Correo");
            else
            {
                var provider = dgrosStore.Providers.SingleOrDefault(p => p.Email.ToLower() == clientEmail.ToLower());

                if(provider != null)
                    return new ValidationResult("El Correo introducido pertenece a un proveedor");
            }

        return ValidationResult.Success;
    }
}
}