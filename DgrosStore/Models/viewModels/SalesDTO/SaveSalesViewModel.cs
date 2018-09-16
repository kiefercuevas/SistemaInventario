using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models.viewModels
{
    public class SaveSalesViewModel
    {
        public IEnumerable<ProductModelForSalesViewDTO> Products;
        public PaymentMethod PaymentMethod;
        public int DiscountType;
        public string Commentary;
        public float Total;
        public int ClientId;
    }
}