using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DgrosStore.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }
        [Required(ErrorMessage = "El nombre del descuento no puede estar vacio")]
        public string DiscountName { get; set; }
        [Required(ErrorMessage = "El Tipo de descuento no puede estar vacio")]
        public string DiscountType { get; set; }

        [Range(0, 100, ErrorMessage = "la cantidad debe estar entre 0 y 100")]
        [Required(ErrorMessage = "El valor del descuento no puede estar vacio")]
        public float Discountvalue { get; set; }


        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}