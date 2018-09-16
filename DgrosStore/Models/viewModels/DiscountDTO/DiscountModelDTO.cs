using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models.viewModels.DiscountDTO
{
    public class DiscountModelDTO
    {
        public int DiscountId { get; set; }
        public string DiscountName { get; set; }
        public string DiscountType { get; set; }
        public float Discountvalue { get; set; }
    }
}