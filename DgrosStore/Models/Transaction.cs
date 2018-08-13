using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public bool State { get; set; }
        public string Commentary { get; set; }
    }
}