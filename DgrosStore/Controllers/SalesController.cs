using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models.viewModels;
using DgrosStore.Models;
using System.Web.Script.Serialization;
namespace DgrosStore.Controllers
{
    public class SalesController : Controller
    {
        private readonly DgrosStoreContext dgrosStore;
        public SalesController()
        {
            dgrosStore = new DgrosStoreContext();
        }
        // GET: Sales
        public ActionResult Index()
        {
            var Sales = new SalesViewModel()
            {
                paymentMethod = new PaymentMethod(),
                products = dgrosStore.Products.ToList(),
                clients = dgrosStore.Clients.ToList(),
            };
            return View("SalesIndex",Sales);
        }

        [Route("Sales/GetClients")]
        public ActionResult GetClients(string client)
        {

            var state = true;
            List<Client> clientInDB;
            
            if (!String.IsNullOrWhiteSpace(client))
            {
                clientInDB = dgrosStore.Clients
                .Where(c => c.Name.IndexOf(client) > -1)
                .Where(c => c.State == state)
                .ToList();

                if(clientInDB.Count() == 0)
                {
                    clientInDB = dgrosStore.Clients
                    .Where(c => c.IdCard.IndexOf(client) > -1)
                    .Where(c => c.State == state)
                    .ToList();
                }
            }
            else
            {
                clientInDB = dgrosStore.Clients
                    .Where(c => c.State == state)
                    .ToList();
            }
            var clientModel = GetClientViewList(clientInDB);
            return Json(clientModel, JsonRequestBehavior.AllowGet);
        }

        [Route("Sales/GetProducts")]
        public ActionResult GetProducts(string product)
        {
            var state = true;
            List<GetProductView> ProductInDB = new List<GetProductView>();
            if (!String.IsNullOrWhiteSpace(product))
            {
                 var products = dgrosStore.Products
                    .Where(p => p.Name.IndexOf(product) > -1)
                    .Where(p => p.State == state)
                    .ToList();

                ProductInDB = GetProductViewList(products);
            }
            else
            {
                var products = dgrosStore.Products.ToList();
                ProductInDB = GetProductViewList(products);
            }
            return Json(ProductInDB, JsonRequestBehavior.AllowGet);
        }

        [Route("Sales/GetProduct")]
        public ActionResult GetProduct(int id)
        {
            //var jsonSerialiser = new JavaScriptSerializer();

            var product = dgrosStore.Products
                        .Select(p => new
                          {
                              id = p.ProductId,
                              name = p.Name,
                              stock = p.Stock,
                              price = p.SellingPrice
                          })
                        .SingleOrDefault(p => p.id == id);
            if (product == null)
                return HttpNotFound();

            //var jsonProduct = jsonSerialiser.Serialize(product);

            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("Sales/Save")]
        public ActionResult Save(string model)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var salesViewModel = jsonSerialiser.Deserialize<SaveSalesViewModel>(model);
            var client = dgrosStore.Clients.SingleOrDefault(c => c.ClientId == salesViewModel.ClientId);

            if (!ModelState.IsValid)
            {
                return Content("false");
            }

            var sales = CreateSales(salesViewModel, client);

            try
            {
                dgrosStore.Sales.Add(sales);
                dgrosStore.SaveChanges();
                return Content("true");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

       
        private Sales CreateSales(SaveSalesViewModel salesViewModel,Client client)
        {
            Sales sales;
            var StoreId = 1;
            sales = new Sales()
            {
                Clients = new List<Client>(),
                StoreId = StoreId,
                Commentary = salesViewModel.commentary,
                Date = DateTime.Now,
                paymentMethod = salesViewModel.paymentMethod,
                State = true,
                SalesProducs = new List<SalesProducs>(),
            };
            
            foreach (var product in salesViewModel.Products)
            {
                var salesProduct = new SalesProducs()
                {
                    Product = dgrosStore.Products.SingleOrDefault(p => p.ProductId == product.id),
                    Discount = product.discount,
                    Quantity = product.quantity,
                    Sales = sales
                };
                sales.SalesProducs.Add(salesProduct);
            };
            if(client != null)
                sales.Clients.Add(client);

            return sales;
        }


        private List<GetProductView> GetProductViewList(List<Product> products)
        {
            var getProductViews = new List<GetProductView>();
            foreach (var item in products)
            {
                var getProduct = new GetProductView
                {
                    ProductId = item.ProductId,
                    Name = item.Name
                };
                getProductViews.Add(getProduct);
            }
            return getProductViews;
        }


        private List<GetClientModelSalesView> GetClientViewList(List<Client> clients)
        {
            var getClientModel = new List<GetClientModelSalesView>();
            foreach (var client in clients)
            {
                var getClient = new GetClientModelSalesView
                {
                   ClientId = client.ClientId,
                   Name = client.Name,
                   LastName = client.LastName
                };
                getClientModel.Add(getClient);
            }
            return getClientModel;
        }
    }
}