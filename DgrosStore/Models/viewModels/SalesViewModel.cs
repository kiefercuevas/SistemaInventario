using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models.viewModels
{
    public class SalesViewModel
    {
       
        /*public Sales Sales;
        public SalesProducs salesProducs;*/
        public int ClientId;
        public IEnumerable<ProductJsonModel> Products;
        public PaymentMethod paymentMethod;
        public string commentary;
        public float Total;

    }
}