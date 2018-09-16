using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models.viewModels;
using DgrosStore.Models;
using System.Web.Script.Serialization;

namespace DgrosStore.Controllers
{
    public class SalesController : Controller
    {
        private readonly DgrosStoreContext dgrosStore;
        public SalesController()
        {
            dgrosStore = new DgrosStoreContext();
        }
       
        public ActionResult Index()
        {
            return View("SalesIndex", CreateSalesViewModel());
        }

        [Route("Sales/GetClients")]
        public ActionResult GetClients(string param)
        {
            List<Client> clients = GetClientsList(param);
            List<ClientModelForSalesViewDTO> clientModel = GetClientViewList(clients);
            return Json(clientModel, JsonRequestBehavior.AllowGet);
        }

        [Route("Sales/GetProducts")]
        public ActionResult GetProducts(string param)
        {
            List<Product> products = GetProductsList(param);
            List<ProductModelForSalesViewDTO> ProductInDB = GetProductViewList(products);
            return Json(ProductInDB, JsonRequestBehavior.AllowGet);
        }

        [Route("Sales/GetProduct")]
        public ActionResult GetProduct(int id)
        {
            ProductDTO product = CreateProductDTO(id);
            if (product == null)
                return Json("0", JsonRequestBehavior.AllowGet);
            else
                return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("Sales/Save")]
        public ActionResult Save(string data)
        {
            JavaScriptSerializer jsonSerialiser = new JavaScriptSerializer();
            SaveSalesViewModel SavesalesViewModel = jsonSerialiser.Deserialize<SaveSalesViewModel>(data);
            Client client = new Client();

            Sales sales = CreateSales(SavesalesViewModel);
            List<SalesProducs> salesProducs = CreateSalesProductList(SavesalesViewModel);

            if (SavesalesViewModel.ClientId != 0)
                sales.Clients.Add(GetClient(SavesalesViewModel.ClientId));

            if(SavesalesViewModel.DiscountType != 0)
                sales.Discounts.Add(GetDiscount(SavesalesViewModel.DiscountType));

            foreach(SalesProducs salesproduct in salesProducs)
                sales.SalesProducs.Add(salesproduct);

            try
            {
                dgrosStore.Sales.Add(sales);
                dgrosStore.SaveChanges();
                return Json("1");
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }

       //aqui hay que pasar el dato de la tienda donde se realizo la venta que lo haremos desde variables de seccion
        private Sales CreateSales(SaveSalesViewModel salesViewModel,bool state = true)
        {
            int StoreId = 1;
            Sales sales = new Sales()
            {
                Clients = new List<Client>(),
                StoreId = StoreId,
                Commentary = salesViewModel.Commentary,
                Date = DateTime.Now,
                paymentMethod = salesViewModel.PaymentMethod,
                State = state,
                SalesProducs = new List<SalesProducs>(),
                Discounts = new List<Discount>(),
                Total = salesViewModel.Total
            };
            return sales;
        }

        private List<SalesProducs> CreateSalesProductList(SaveSalesViewModel saveSalesViewModel = null)
        {
            List<SalesProducs> salesProducsList = new List<SalesProducs>();
            if (saveSalesViewModel == null)
                return salesProducsList;
            else
            {
                foreach (ProductModelForSalesViewDTO product in saveSalesViewModel.Products)
                {
                    SalesProducs salesProduct = new SalesProducs()
                    {
                        Product = GetProductById(product.ProductId),
                        Discount = product.Discount,
                        Quantity = product.Quantity,
                        UnitPrice = GetProductById(product.ProductId).SellingPrice,
                        SubTotal = product.SubTotal,
                    };
                    salesProducsList.Add(salesProduct);
                };
                return salesProducsList;
            }
        }

        private SalesViewModel CreateSalesViewModel(bool state = true)
        {
            string defaultCommentary = "gracias por su compra";
            SalesViewModel salesViewModel = new SalesViewModel()
            {
                PaymentMethod = new PaymentMethod(),
                Products = dgrosStore.Products.Where(p => p.State == state).ToList(),
                Clients = dgrosStore.Clients.Where(p => p.State == state).ToList(),
                Discounts = dgrosStore.Discounts.ToList(),
                commentary = defaultCommentary
            };
            return salesViewModel;
        }

        private List<ProductModelForSalesViewDTO> GetProductViewList(List<Product> products)
        {
            var getProductViews = new List<ProductModelForSalesViewDTO>();
            foreach (var item in products)
            {
                var getProduct = new ProductModelForSalesViewDTO
                {
                    ProductId = item.ProductId,
                    Name = item.Name
                };
                getProductViews.Add(getProduct);
            }
            return getProductViews;
        }


        private List<Client> GetClientsList(string param,bool state = true)
        {
            if(String.IsNullOrEmpty(param))
                return dgrosStore.Clients.Where(c => c.State == state).ToList();
            else
                return dgrosStore.Clients.Where(c => c.State == state)
                    .Where(c => c.Name.IndexOf(param) > -1 || c.IdCard.IndexOf(param) > -1).ToList();
        }
        private List<Product> GetProductsList(string param, bool state = true)
        {
            if (String.IsNullOrEmpty(param))
                return dgrosStore.Products.Where(p => p.State == state).ToList();
            else
                return dgrosStore.Products.Where(p => p.State == state)
                    .Where(p => p.Name.IndexOf(param) > -1).ToList();
        }

        private ProductDTO CreateProductDTO(int id,bool state = true)
        {
            Product product = dgrosStore.Products
                .Where(p => p.ProductId == id).Where(p => p.State == state).SingleOrDefault();

            if (product != null)
            {
                ProductDTO productDTO = new ProductDTO()
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Stock = product.Stock,
                    SellingPrice = product.SellingPrice
                };
                return productDTO;
            }
            else
                return null;
        }

        private List<ClientModelForSalesViewDTO> GetClientViewList(List<Client> clients)
        {
            List<ClientModelForSalesViewDTO> clietForSalesModelList = new List<ClientModelForSalesViewDTO>();
            ClientModelForSalesViewDTO clientforSalesModel = new ClientModelForSalesViewDTO();

            foreach (Client client in clients)
            {
                clientforSalesModel.ClientId = client.ClientId;
                clientforSalesModel.Name = client.Name;
                clientforSalesModel.LastName = client.LastName;

                clietForSalesModelList.Add(clientforSalesModel);
            }
            return clietForSalesModelList;
        }

        private Client GetClient(int id)
        {
            Client client = dgrosStore.Clients.SingleOrDefault(c => c.ClientId == id);
            return client;
        }

        private Product GetProductById(int id)
        {
            return dgrosStore.Products.Where(p => p.ProductId == id).SingleOrDefault();
        }
        private Discount GetDiscount(int id)
        {
            return dgrosStore.Discounts.Where(d => d.DiscountId == id).SingleOrDefault();
        }

    }
}