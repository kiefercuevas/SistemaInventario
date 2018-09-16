using System;
using System.Collections.Generic;
using System.Linq;
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

            var Datatable = new DataTable()
            {
                Draw = Convert.ToInt32(Request["draw"]),
                Start = Convert.ToInt32(Request["start"]),
                Length = Convert.ToInt32(Request["length"]),
                Search = Request["search[value]"],
                Order = Request["columns[" + Request["order[0][column]"] + "][name]"],
                OrderDir = Request["order[0][dir]"],
                RecordsTotal = Clients.Count(),
                RecordFiltered = 0
            };

            if (!String.IsNullOrEmpty(Datatable.Search))
            {
                Clients = dgrosStore.Clients
                .Where(c => c.State == state)
                .Where(c => c.Name.ToLower().IndexOf(Datatable.Search) > -1 ||
                            c.IdCard.IndexOf(Datatable.Search) > -1 ||
                            c.Email.IndexOf(Datatable.Search) > -1 ||
                            c.Telephones.FirstOrDefault(t => t.Number.Length > 0).Number.ToString().IndexOf(Datatable.Search) > -1)
                .OrderBy(c => c.Name)
                .ToList();
            }

            Datatable.RecordFiltered = Clients.Count();

            //order,paging
            var productOrdered = OrderProductsByParam(Clients, Datatable.Order, Datatable.OrderDir);
            var paginClients = productOrdered.Skip(Datatable.Start).Take(Datatable.Length).ToList();
            var clientModelList = CreateClientModelList(paginClients);

            var clientModelToDatatable = new
            {
                draw = Datatable.Draw,
                recordsTotal = Datatable.RecordsTotal,
                recordsFiltered = Datatable.RecordFiltered,
                data = clientModelList
            };

            return Json(clientModelToDatatable, JsonRequestBehavior.AllowGet);
        }

        [Route("Client/details/{id}")]
        public ActionResult ClientDetails(int id)
        {
            var state = true;
            var Client = dgrosStore.Clients
                .Include(c => c.Telephones)
                .Include(c => c.Sales)
                .Where(c => c.State == state)
                .SingleOrDefault(c => c.PersonId == id);

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
            var type = "client";

            if (clientView.Client.PersonId == 0)
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
                        clientView.Client.Image = data.RelativePath;
                        clientView.UploadedFile.SaveAs(data.AbsolutePath);
                    }
                    else
                        clientView.Client.Image = url + "client.png";

                    if (!String.IsNullOrWhiteSpace(clientView.Telephone))
                    {
                        clientView.Client.Telephones = new List<Telephone>
                        {
                            new Telephone()
                            {
                                Number = clientView.Telephone,
                                Person = clientView.Client
                            }
                        };
                    }
                    clientView.Client.State = state;
                    clientView.Client.Type = type.ToLower();
                    dgrosStore.Clients.Add(clientView.Client);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else
            {
                var clientInDb = dgrosStore.Clients.SingleOrDefault(c => c.PersonId == clientView.Client.PersonId);

                if (!ModelState.IsValid)
                {
                    var editClient = new ClientViewModel()
                    {
                        Client = clientInDb,
                    };
                    return View("SaveClient", editClient);
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
                        if (data.RelativePath != clientInDb.Image)
                        {
                            clientInDb.Image = data.RelativePath;
                            clientView.UploadedFile.SaveAs(data.AbsolutePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message);
                    }
                }
            }
            try
            {
                dgrosStore.SaveChanges();
                return RedirectToAction("Index", "Client");
            }catch(DbEntityValidationException ex)
            {
                string data = "";
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var err in error.ValidationErrors)
                    {
                        data += err.PropertyName + "-" + err.ErrorMessage; 
                    }
                }
                return Content(data);
            }
            
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
                .SingleOrDefault(c => c.PersonId == id);

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
            var client = dgrosStore.Clients.Include(c => c.Sales).SingleOrDefault(p => p.PersonId == id);
            var error = "";
            if (client == null)
                return Json("0");
            else
            {
                try
                {

                    foreach (var sale in client.Sales)
                        sale.State = state;

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
                RelativePath = url + filename,
                //ruta absoluta
                AbsolutePath = path
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
                    ClientId = client.PersonId,
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

        private List<Client> OrderProductsByParam(List<Client> clients, string order, string direction)
        {
            if (!String.IsNullOrWhiteSpace(order) && !String.IsNullOrWhiteSpace(direction))
            {
                if (direction == "asc")
                    clients = SwitchStructureAsc(clients, order);
                else if (direction == "desc")
                    clients = SwitchStructureDesc(clients, order);
            }
            return clients;
        }

        private List<Client> SwitchStructureAsc(List<Client> clients, string order)
        {
            switch (order)
            {
                case "Name": clients = clients.OrderBy(p => p.Name).ToList(); return clients;
                case "IdCard": clients = clients.OrderBy(p => p.IdCard).ToList(); return clients;
                case "Email": clients = clients.OrderBy(p => p.Email).ToList(); return clients;
                case "Stock": clients = clients.OrderBy(p => p.Telephones.FirstOrDefault().Number.ToString()).ToList(); return clients;
                default: clients = clients.ToList(); return clients;
            }
        }
        private List<Client> SwitchStructureDesc(List<Client> clients, string order)
        {

            switch (order)
            {
                case "Name": clients = clients.OrderByDescending(p => p.Name).ToList(); return clients;
                case "IdCard": clients = clients.OrderByDescending(p => p.IdCard).ToList(); return clients;
                case "Email": clients = clients.OrderByDescending(p => p.Email).ToList(); return clients;
                case "Stock": clients = clients.OrderByDescending(p => p.Telephones.FirstOrDefault().Number.ToString()).ToList(); return clients;
                default: clients = clients.ToList(); return clients;
            }
        }
    }
}
