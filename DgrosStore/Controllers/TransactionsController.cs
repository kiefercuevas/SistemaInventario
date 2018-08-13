using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models.viewModels;
using DgrosStore.Models;
namespace DgrosStore.Controllers
{
    public class TransactionsController : Controller
    {
        public List<Transaction> Transactions { get; set; }

        public TransactionsController()
        {
            /*Transactions = new List<Transaction>()
            {
              new Transaction()
              { Id=1,Type="Venta",ReleaseDate=DateTime.Now,
                        Products = new List<Product>()
                        {
                            new Product { Id=1,Name="Camiseta",Price=240.95f,Quantity=1 , Description = "Una camiseta excelente de gran calidad"},
                            new Product { Id=2,Name="Pantalon",Price=550.99f,Quantity=2, Description = "Un pantalon a otro nivel"},
                        },
              },
              new Transaction()
              { Id=2,Type="Compra",ReleaseDate=DateTime.Now,
                        Products = new List<Product>()
                        {
                            new Product { Id=1,Name="Camiseta",Price=240.95f,Quantity=4 , Description = "Una camiseta excelente de gran calidad"},
                            new Product { Id=2,Name="Pantalon",Price=550.99f,Quantity=3, Description = "Un pantalon a otro nivel"},
                        },
              }


            };
            Transactions.First().Total = Transactions.First().Products.Sum(p => p.Price *p.Quantity);
            Transactions.Last().Total = Transactions.Last().Products.Sum(p => p.Price * p.Quantity);*/
        }

        // GET: Transactions

        public ActionResult Index()
        {
             

            return View(Transactions);
        }
        
        [Route("Transaction/Details/{id}")]
        public ActionResult TransactionDetails(int id)
        {
            var transaction = Transactions.SingleOrDefault(t => t.Id == id);
            return View(transaction);
        }

    }
}