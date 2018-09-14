using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;

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
            var person = dgrosStore.People
                .Where(p => p.PersonId == id)
                .FirstOrDefault();

            if (person == null)
                return HttpNotFound();

            var telephone = new Telephone()
            {
                PersonId = id,
                Person = person
            };
            return View("SaveTelephone", telephone);
        }

        public ActionResult Edit(int id)
        {
            var telephone = dgrosStore.Telephones
                .Include(t => t.Person)
                .Where(t => t.TelephoneId == id)
                .SingleOrDefault();

            if (telephone == null)
                return HttpNotFound();
            else
                return View("SaveTelephone", telephone);

                
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Telephone telephone)
        {
            var person = dgrosStore.People
                .Where(p => p.PersonId == telephone.PersonId)
                .FirstOrDefault();
            telephone.Person = person;

            if (telephone.TelephoneId == 0)
            {
                if (!ModelState.IsValid)
                {
                    var emptyTelephone = new Telephone()
                    {
                        PersonId = telephone.PersonId,
                        Person = person
                    };
                    return View("SaveTelephone", emptyTelephone);
                }
                if (!String.IsNullOrWhiteSpace(telephone.Number))
                    dgrosStore.Telephones.Add(telephone);
                    
            }   
            else
            {
                var telephoneInDb = dgrosStore.Telephones
                    .Include(t => t.Person)
                    .SingleOrDefault(t => t.TelephoneId == telephone.TelephoneId);

                if (!ModelState.IsValid)
                {
                    var editTelephone = telephoneInDb;
                    return View("SaveTelephone", editTelephone);
                }

                if (telephoneInDb != null)
                {
                    if (!String.IsNullOrWhiteSpace(telephone.Number))
                        telephoneInDb.Number = telephone.Number;
                       
                }
            }
            dgrosStore.SaveChanges();
            if(telephone.Person.Type == "client")
                return RedirectToAction("ClientDetails", "Client", new { id = telephone.PersonId });
            else
                return RedirectToAction("ProviderDetails", "Provider", new { id = telephone.PersonId });
        }

        public ActionResult Delete(int id)
        {
            var error = "";
            var Telephone = dgrosStore.Telephones
                .Where(t => t.TelephoneId == id)
                .SingleOrDefault();

            if (Telephone == null)
                return Json("0");
            else
            {
                try
                {
                    dgrosStore.Telephones.Remove(Telephone);
                    dgrosStore.SaveChanges();
                    return Json("1");
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            error += String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    return Json(error);
                }
            }

        }

    }
}