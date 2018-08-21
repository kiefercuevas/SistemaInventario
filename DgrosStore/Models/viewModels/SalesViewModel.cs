using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models.viewModels
{
    public class SalesViewModel
    {

        [Required(ErrorMessage = "Debe de elegir un metodo de pago")]
        public PaymentMethod paymentMethod;
        public IEnumerable<Client> clients;
        [Required(ErrorMessage = "Debe de elegir al menos un producto")]
        public IEnumerable<Product> products;
        public string commentary;
    }
}