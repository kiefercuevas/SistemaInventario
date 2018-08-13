using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models;
namespace DgrosStore.Controllers
{
    public class TelephoneController : Controller
    {
        private readonly DgrosStoreContext dgrosStore;

        public TelephoneController()
        {
            dgrosStore = new DgrosStoreContext();
        }

        
        public ActionResult Create(int id)
        {
            var telephone = new Telephone()
            {
                ClientId = id
            };
            return View("SaveTelephone", telephone);
        }

        public ActionResult Edit(int id)
        {
            var telephone = dgrosStore.Telephones
                .SingleOrDefault(t => t.TelephoneId == id);

            if (telephone == null)
                return HttpNotFound();
            else
                return View("SaveTelephone",telephone);
        }

        public ActionResult Save(Telephone telephone)
        {
            if(telephone.TelephoneId == 0)
            {
                if (!String.IsNullOrWhiteSpace(telephone.Number))
                    dgrosStore.Telephones.Add(telephone);   
            }   
            else
            {
                var telephoneInDb = dgrosStore.Telephones.SingleOrDefault(t => t.TelephoneId == telephone.TelephoneId);
                if(telephoneInDb != null)
                {
                    if (!String.IsNullOrWhiteSpace(telephone.Number))
                        telephoneInDb.Number = telephone.Number;
                       
                }
            }

            dgrosStore.SaveChanges();
            return RedirectToAction("ClientDetails", "Client", new { id = telephone.ClientId});
        }

        public ActionResult Delete(int id)
        {
            return Content("working");
        }

    }
}