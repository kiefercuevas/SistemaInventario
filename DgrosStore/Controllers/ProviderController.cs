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

        [Route("ProviderDetails/{id}")]
        public ActionResult ProviderDetails(int id)
        {
            var provider = dgrosStore.Providers.SingleOrDefault(p => p.ProviderId == id);
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
            var providers = dgrosStore.Providers
                .Where(p => p.State == true)
                .ToList();

            var providerModel = CreateClientModelList(providers);

            return Json(providerModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var provider = new Provider();
            return View("SaveProvider", provider);
        }

        public ActionResult Edit(int id)
        {
            var provider = dgrosStore.Providers.SingleOrDefault(p => p.ProviderId == id);
            if (provider == null)
                return HttpNotFound();
            else
                return View("SaveProvider", provider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Provider provider)
        {
            var state = true;
            if(provider.ProviderId == 0)
            {
                if (!ModelState.IsValid)
                {
                    var emptyProdider = new Provider();
                    return View("SaveProvider", emptyProdider);
                }
                provider.State = state;
                dgrosStore.Providers.Add(provider);
            }
            else
            {
                var providerInDb = dgrosStore.Providers.SingleOrDefault(p => p.ProviderId == provider.ProviderId);

                if (!ModelState.IsValid)
                {
                    var editProdider = providerInDb;
                    return View("SaveProvider", editProdider);
                }

                providerInDb.Name = provider.Name;
                providerInDb.Telephone = provider.Telephone;
                providerInDb.Email = provider.Email;
                providerInDb.Direcction = provider.Direcction;

            }

            dgrosStore.SaveChanges();
            return RedirectToAction("Index","Provider");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var state = false;
            var provider = dgrosStore.Providers.Include(p => p.Shoppings).SingleOrDefault(p => p.ProviderId == id);
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


        private List<GetProvidersModel> CreateClientModelList(List<Provider> providers)
        {
            var providersModelList = new List<GetProvidersModel>();
            foreach (var provider in providers)
            {
                var ProviderModel = new GetProvidersModel()
                {
                    ProviderId = provider.ProviderId,
                    Name = provider.Name,
                    Email = provider.Email,
                    Telephone = provider.Telephone
                };
                providersModelList.Add(ProviderModel);
            }

            return providersModelList;
        }

    }
}