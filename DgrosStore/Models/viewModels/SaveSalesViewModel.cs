using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models.viewModels
{
    public class SaveSalesViewModel
    {
        public int ClientId;
        public IEnumerable<ProductJsonModel> Products;
        public PaymentMethod paymentMethod;
        public string commentary;
        public float Total;
    }
}