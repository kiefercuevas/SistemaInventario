using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models.viewModels
{
    public class ClientModelForSalesViewDTO
    {
        public int ClientId { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }
    }
}