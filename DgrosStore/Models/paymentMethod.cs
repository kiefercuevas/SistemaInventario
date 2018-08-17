using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models
{
    public enum PaymentMethod
    {
        [Display(Name ="Dinero en efectivo")]
        Money=1,
        [Display(Name = "Transferencia bancaria")]
        WireTransfer,
        [Display(Name = "Pago con tarjeta")]
        CardPayment
    }
}