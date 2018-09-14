using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace DgrosStore.Models.customValidation
{
    public class PersonEmailValidation : ValidationAttribute
    {
        private readonly DgrosStoreContext dgrosStore;

    public PersonEmailValidation()
    {
        dgrosStore = new DgrosStoreContext();
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var person = (Person)validationContext.ObjectInstance;
        var personEmail = person.Email;
        
        var personInDb = dgrosStore.People
                .SingleOrDefault(c => c.Email.ToLower() == personEmail.ToLower());


            if (personInDb != null && personInDb.PersonId != person.PersonId && personInDb.Type == "client")
            {
                return new ValidationResult("Ya existe un cliente con este Correo ");
            }else if(personInDb != null && personInDb.PersonId != person.PersonId && personInDb.Type == "provider")
            {
                return new ValidationResult("Ya existe un proveedor con este Correo ");
            }
                

        return ValidationResult.Success;
    }
}
}