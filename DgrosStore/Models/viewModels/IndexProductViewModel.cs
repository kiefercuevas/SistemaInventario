using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models.viewModels
{
    public class IndexProductViewModel :PaginationModel
    {
        public List<Product> Products { get; set; }
    }
}