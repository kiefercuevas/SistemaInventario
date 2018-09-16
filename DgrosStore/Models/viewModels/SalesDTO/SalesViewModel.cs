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
        public PaymentMethod PaymentMethod;
        public IEnumerable<Client> Clients;
        [Required(ErrorMessage = "Debe de elegir al menos un producto")]
        public IEnumerable<Product> Products;

        public IEnumerable<Discount> Discounts;
        public string commentary;
    }
}