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
                paymentMethod = new PaymentMethod()
            };
            return View("SalesIndex",Sales);
        }

        [Route("Sales/GetClients")]
        public ActionResult GetClients(string client)
        {

            var jsonSerialiser = new JavaScriptSerializer();
            var clientInDB = dgrosStore.Clients
                .Where(c => c.Name.IndexOf(client) > -1)
                .Select(c => new
                {
                    id = c.ClientId,
                    name = c.Name,
                    lastName = c.LastName
                }).ToList();


            var JsonClients = jsonSerialiser
                .Serialize(clientInDB);

            return Json(JsonClients, JsonRequestBehavior.AllowGet);
        }

        [Route("Sales/GetProducts")]
        public ActionResult GetProducts(string product)
        {

            var jsonSerialiser = new JavaScriptSerializer();
            var productInDB = dgrosStore.Products
                .Where(p => p.Name.IndexOf(product) > -1)
                .Select(p => new
                {
                    id = p.ProductId,
                    name = p.Name,
                }).ToList();


            var JsonProduct = jsonSerialiser
                .Serialize(productInDB);

            return Json(JsonProduct, JsonRequestBehavior.AllowGet);
        }

        [Route("Sales/GetProduct")]
        public ActionResult GetProduct(int id)
        {
            var jsonSerialiser = new JavaScriptSerializer();

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
            {
                return HttpNotFound();
            }

            var jsonProduct = jsonSerialiser.Serialize(product);

            return Json(jsonProduct,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("Sales/Save")]
        public ActionResult Save(string model)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var salesViewModel = jsonSerialiser.Deserialize<SalesViewModel>(model);
            var client = dgrosStore.Clients.SingleOrDefault(c => c.ClientId == salesViewModel.ClientId);

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

       
        private Sales CreateSales(SalesViewModel salesViewModel,Client client)
        {
            Sales sales;

            sales = new Sales()
            {
                Clients = new List<Client>(),
                StoreId = 1,
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

            sales.Clients.Add(client);

            return sales;
        }

    }
}