using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models;
using System.Data.Entity;
using DgrosStore.Models.viewModels;
using System.IO;
using System.Data.Entity.Validation;

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

            return View("ClientIndex");
        }

        public ActionResult GetClients()
        {
            var state = true;
            var Clients = dgrosStore.Clients
                .Include(c => c.Telephones)
                .Where(c => c.State == state)
                .ToList();

            var clientModelList = CreateClientModelList(Clients);

            return Json(clientModelList, JsonRequestBehavior.AllowGet);
        }

        [Route("Client/details/{id}")]
        public ActionResult ClientDetails(int id)
        {
            var state = true;
            var Client = dgrosStore.Clients
                .Include(c => c.Telephones)
                .Include(c => c.Sales)
                .Where(c => c.State == state)
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

            return View("SaveClient", client);
        }

        [HttpPost]
        [Route("Save/Client")]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ClientViewModel clientView)
        {
            string url = "/Content/Images/Clients/";
            var state = true;

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

                    if (!String.IsNullOrWhiteSpace(clientView.Telephone))
                    {
                        clientView.Client.Telephones = new List<Telephone>();
                        clientView.Client.Telephones.Add(new Telephone()
                        {
                            Number = clientView.Telephone,
                            Client = clientView.Client
                        });
                    }
                    clientView.Client.State = state;
                    dgrosStore.Clients.Add(clientView.Client);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else
            {
                var clientInDb = dgrosStore.Clients.SingleOrDefault(c => c.ClientId == clientView.Client.ClientId);

                if (!ModelState.IsValid)
                {
                    var editClient = new ClientViewModel()
                    {
                        Client = clientInDb,
                    };
                    /*var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new { x.Key, x.Value.Errors })
                        .ToList();
                    var error = errors.FirstOrDefault(e => e.Key.IndexOf("T") > -1);
                    var errorlist = error.Errors.FirstOrDefault(e => true);*/
                    return View("SaveClient", editClient);
                    //return Content(errorlist.ErrorMessage);
                }

                clientInDb.Name = clientView.Client.Name;
                clientInDb.LastName = clientView.Client.LastName;
                clientInDb.Email = clientView.Client.Email;
                clientInDb.Direcction = clientView.Client.Direcction;
                clientInDb.IdCard = clientView.Client.IdCard;

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
            var state = true;
            var ClientInDb = dgrosStore.Clients
                .Where(c => c.State == state)
                .SingleOrDefault(c => c.ClientId == id);

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
        
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var state = false;
            var client = dgrosStore.Clients.SingleOrDefault(p => p.ClientId == id);
            var error = "";
            if (client == null)
                return Json("0");
            else
            {
                try
                {
                    client.State = state;
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

        private List<GetClientModel> CreateClientModelList(List<Client> clients)
        {
            var clientModelList = new List<GetClientModel>();
            foreach (var client in clients)
            {
                var telephone = client.Telephones
                    .FirstOrDefault(c => c.Number.Length > 0);

                var clientModel = new GetClientModel()
                {
                    ClientId = client.ClientId,
                    Name = client.Name,
                    Email = client.Email,
                    IdCard = client.IdCard
                };
                if (telephone == null)
                    clientModel.Telephone = "";
                else
                    clientModel.Telephone = telephone.Number.ToString();

                
               
                clientModelList.Add(clientModel);
            }

            return clientModelList;
        }
    }
}