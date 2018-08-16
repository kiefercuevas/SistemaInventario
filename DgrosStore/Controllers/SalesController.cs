using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models.viewModels;
using DgrosStore.Models;
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
    }
}