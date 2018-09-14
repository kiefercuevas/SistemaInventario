using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using DgrosStore.Models;
using DgrosStore.Models.viewModels;
using System.Data.Entity;

namespace DgrosStore.Controllers
{
    public class ProviderController : Controller
    {
        // GET: Provider
        private readonly DgrosStoreContext dgrosStore;

        public ProviderController()
        {
            dgrosStore = new DgrosStoreContext();
        }

        [Route("Provider/details/{id}")]
        public ActionResult ProviderDetails(int id)
        {
            bool state = true;
            var provider = dgrosStore.Providers
                .Include(p => p.Telephones)
                .Include(p => p.Shoppings)
                .Where(p => p.State == state)
                .SingleOrDefault(p => p.PersonId == id);

            if (provider == null)
                return HttpNotFound();
            else
                return View("ProviderDetails", provider);
        }

        public ActionResult Index()
        {
            
            return View("ProviderIndex");
        }

        public ActionResult GetProviders()
        {
            var state = true;
            var providers = dgrosStore.Providers
                .Include(p => p.Telephones)
                .Where(p => p.State == state)
                .ToList();

            var providerModel = CreateProviderModelList(providers);

            return Json(providerModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var ProviderModel = new ProviderViewModel()
            {
                Provider = new Provider()
            };
            return View("SaveProvider", ProviderModel);
        }

        public ActionResult Edit(int id)
        {
            var provider = dgrosStore.Providers.SingleOrDefault(p => p.PersonId == id);
            if (provider == null)
                return HttpNotFound();
            else
            {
                var ProviderModel = new ProviderViewModel()
                {
                    Provider = provider
                };
                return View("SaveProvider", ProviderModel);
            }     
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ProviderViewModel providerView)
        {
            var state = true;
            var type = "provider";

            if (providerView.Provider.PersonId == 0)
            {
                if (!ModelState.IsValid)
                {
                    var emptyProvider = new ProviderViewModel()
                    {
                        Provider = new Provider()
                    };
                    return View("SaveProvider", emptyProvider);
                }

                if (!String.IsNullOrWhiteSpace(providerView.Telephone))
                {
                    providerView.Provider.Telephones = new List<Telephone>();
                    providerView.Provider.Telephones.Add(new Telephone()
                    {
                        Number = providerView.Telephone,
                        Person = providerView.Provider
                    });
                }
                providerView.Provider.State = state;
                providerView.Provider.Type = type;
                dgrosStore.Providers.Add(providerView.Provider);
            }
            else
            {
                var providerInDb = dgrosStore.Providers.SingleOrDefault(p => p.PersonId == providerView.Provider.PersonId);

                if (!ModelState.IsValid)
                {
                    var editProdider = new ProviderViewModel()
                    {
                        Provider = providerInDb
                    };
                    return View("SaveProvider", editProdider);
                }

                providerInDb.Name = providerView.Provider.Name;
                providerInDb.Email = providerView.Provider.Email;
                providerInDb.Direcction = providerView.Provider.Direcction;
                providerInDb.Rnc = providerView.Provider.Rnc;

            }
            try
            {
                dgrosStore.SaveChanges();
                return RedirectToAction("Index", "Provider");
            }
            catch (DbEntityValidationException dbEx)
            {
                string error = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        error += String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return Content(error);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var state = false;
            var provider = dgrosStore.Providers
                .Include(p => p.Shoppings).SingleOrDefault(p => p.PersonId == id);

            var error = "";
            if (provider == null)
                return Json("0");
            else
            {
                try
                {

                    foreach (var shopping in provider.Shoppings)
                        shopping.State = state;

                    provider.State = state;
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


        private List<GetProvidersModel> CreateProviderModelList(List<Provider> providers)
        {
            var providersModelList = new List<GetProvidersModel>();
            foreach (var provider in providers)
            {
                var telephone = provider.Telephones
                    .Where(t => t.Number.Length > 0).FirstOrDefault();

                var ProviderModel = new GetProvidersModel()
                {
                    ProviderId = provider.PersonId,
                    Name = provider.Name,
                    Email = provider.Email,
                    Rnc = provider.Rnc,
                };
                if (telephone == null)
                    ProviderModel.Telephone = " ";
                else
                    ProviderModel.Telephone = telephone.Number;

                providersModelList.Add(ProviderModel);
            }

            return providersModelList;
        }

    }
}