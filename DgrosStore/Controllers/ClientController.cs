using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models;
using System.Data.Entity;
using DgrosStore.Models.viewModels;
using System.IO;
namespace DgrosStore.Controllers
{
    public class ClientController : Controller
    {
        private readonly DgrosStoreContext dgrosStore;
        // GET: Client
        public ClientController()
        {
            dgrosStore = new DgrosStoreContext();
        }
        public ActionResult Index()
        {
            var Clients = dgrosStore.Clients
                .Include(c => c.Telephones);

            return View("ClientIndex",Clients);
        }

        [Route("Client/details/{id}")]
        public ActionResult ClientDetails(int id)
        {
            var Client = dgrosStore.Clients
                .Include(c => c.Telephones)
                .Include(c => c.Sales)
                .SingleOrDefault(c => c.ClientId == id);

            if (Client == null)
                return HttpNotFound();
            else
                return View(Client);
        }

        [Route("Create/Client")]
        public ActionResult Create()
        {
            var client = new ClientViewModel()
            {
                Client = new Client(),
            };

            return View("SaveClient",client);
        }

        [HttpPost]
        [Route("Save/Client")]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ClientViewModel clientView)
        {
            string url = "/Content/Images/Clients/";

            if (clientView.Client.ClientId == 0)
            {
                if (!ModelState.IsValid)
                {
                    var emptyClient = new ClientViewModel()
                    {
                        Client = new Client()
                    };
                    return View("SaveClient", emptyClient);
                }
                try
                {
                    if (clientView.UploadedFile != null)
                    {
                        Image data = UploadMethod(clientView, url);
                        clientView.Client.Image = data.ImagePath;
                        clientView.UploadedFile.SaveAs(data.CompletePath);
                    }
                    else
                        clientView.Client.Image = url + "client.png";

                    if(!String.IsNullOrWhiteSpace(clientView.Telephone))
                    {
                        clientView.Client.Telephones = new List<Telephone>();
                        clientView.Client.Telephones.Add(new Telephone()
                        {
                            Number = clientView.Telephone,
                            Client = clientView.Client
                        });
                    }
                    dgrosStore.Clients.Add(clientView.Client);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else
            {
                var clientInDb = dgrosStore.Clients.SingleOrDefault(c=> c.ClientId == clientView.Client.ClientId);

                if (!ModelState.IsValid)
                {
                    var editClient = new ClientViewModel()
                    {
                        Client = clientInDb,
                        Telephone = clientView.Telephone
                    };
                    return View("SaveClient", editClient);
                }

                clientInDb.Name = clientView.Client.Name;
                clientInDb.LastName = clientView.Client.LastName;
                clientInDb.Email = clientView.Client.Email;
                clientInDb.Direcction = clientView.Client.Direcction;

                if (clientView.UploadedFile != null)
                {
                    try
                    {
                        Image data = UploadMethod(clientView, url);
                        if (data.ImagePath != clientInDb.Image)
                        {
                            clientInDb.Image = data.ImagePath;
                            clientView.UploadedFile.SaveAs(data.CompletePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message);
                    }
                }
            }

            dgrosStore.SaveChanges();
            return RedirectToAction("Index", "Client");
        }

        protected override void Dispose(bool disposing)
        {
            dgrosStore.Dispose();
        }

        [Route("Edit/Client/{id}")]
        public ActionResult Edit(int id)
        {
            var ClientInDb = dgrosStore.Clients.SingleOrDefault(c => c.ClientId == id);

            if (ClientInDb == null)
                return HttpNotFound();
            else
            {
                var client = new ClientViewModel()
                {
                    Client = ClientInDb
                };
                return View("SaveClient", client);
            }

        }


        private Image UploadMethod(ClientViewModel clientView, string url)
        {
            //para obtener el nombre y la extension del archivo
            string filename = Path.GetFileName(clientView.UploadedFile.FileName);
            var path = Path.Combine(Server.MapPath(url) + filename);

            return new Image()
            {
                //imagePath es la ruta relativa
                ImagePath = url + filename,
                //ruta absoluta
                CompletePath = path
            };
        }
    }
}