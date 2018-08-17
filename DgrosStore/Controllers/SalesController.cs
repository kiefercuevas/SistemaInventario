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
                Clients = dgrosStore.Clients.ToList(),
                Products = dgrosStore.Products.ToList(),
                Sales = new Sales(),
                salesProducs = new SalesProducs()
                
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
    }
}