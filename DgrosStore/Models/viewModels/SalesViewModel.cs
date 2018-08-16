using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgrosStore.Models.viewModels
{
    public class SalesViewModel
    {
        public IEnumerable<Client> Clients;
        public Client SelectedClient;

        public IEnumerable<Product> Products;
        public Product selectedProduct;

        public Sales Sales;
        public SalesProducs salesProducs;

    }
}