using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models;
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
            var providers = dgrosStore.Providers.ToList();
            return View("ProviderIndex",providers);
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

        public ActionResult Save(Provider provider)
        {
            if(provider.ProviderId == 0)
            {
                if (!ModelState.IsValid)
                {
                    var emptyProdider = new Provider();
                    return View("SaveProvider", emptyProdider);
                }
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

    }
}