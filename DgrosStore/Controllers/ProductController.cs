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
    public class ProductController : Controller
    {
        private readonly DgrosStoreContext dgrosStore;

        public ProductController()
        {
            dgrosStore = new DgrosStoreContext();
        }

        public ActionResult Index()
        {
            return View("ProductIndex");
        }
        public ActionResult GetProducts()
        {
            bool state = true;
            List<Product> Products = dgrosStore.Products
                .Where(p => p.State == state).OrderBy(p => p.ProductId).ToList();
            DataTable Datatable = CreateDatatable(Request);

            Datatable.RecordsTotal = Products.Count();

            if (!String.IsNullOrEmpty(Datatable.Search))
            {
                Products = dgrosStore.Products
                .Where(p => p.State == state)
                .Where(p => p.Name.ToLower().IndexOf(Datatable.Search) > -1 ||
                            p.ShoppingPrice.ToString().IndexOf(Datatable.Search) > -1 ||
                            p.SellingPrice.ToString().IndexOf(Datatable.Search) > -1 ||
                            p.Stock.ToString().IndexOf(Datatable.Search) > -1)
                .OrderBy(p => p.CategoryId)
                .ToList();
            }

            Datatable.RecordFiltered = Products.Count();

            //order,paging
            List<Product>productOrdered = OrderProductsByParam(Products, Datatable.Order,Datatable.OrderDir);
            List<Product> pagingProducts = productOrdered.Skip(Datatable.Start).Take(Datatable.Length).ToList();
            List<ProductDTO> productDTOlist = CreateProductModelList(pagingProducts);

            return Json(
               new
               {
                   draw = Datatable.Draw,
                   recordsTotal = Datatable.RecordsTotal,
                   recordsFiltered = Datatable.RecordFiltered,
                   data = productDTOlist
               }
               , JsonRequestBehavior.AllowGet);
        }
   
        [Route("Product/details/{id}")]
        public ActionResult ProductDetails(int id)
        {
            bool state = true;
            Product product = dgrosStore.Products
                .Include(p => p.Category)
                .Where(p => p.State == state)
                .SingleOrDefault(p => p.ProductId == id);

            if (product == null)
                return HttpNotFound();
            else
                return View(product);
        }

        [Route("Product/Category/{category}")]
        public ActionResult ProductByCategory(string category)
        {
            ViewData["category"] = category;
            return View();
        }

        public ActionResult GetProductByCategory(string category)
        {
            bool state = true;
            List<Product> categoryProducts = dgrosStore.Products
                .Include(p => p.Category)
                .Where(p => p.Category.Name == category)
                .Where(p => p.State == state)
                .ToList();

            DataTable Datatable = CreateDatatable(Request);
            Datatable.RecordsTotal = categoryProducts.Count();

            if (!String.IsNullOrEmpty(Datatable.Search))
            {
                categoryProducts = dgrosStore.Products
                    .Include(p => p.Category)
                .Where(p => p.Category.Name == category)
                .Where(p => p.State == state)
                .Where(p => p.Name.ToLower().IndexOf(Datatable.Search) > -1 ||
                            p.ShoppingPrice.ToString().IndexOf(Datatable.Search) > -1 ||
                            p.SellingPrice.ToString().IndexOf(Datatable.Search) > -1 ||
                            p.Stock.ToString().IndexOf(Datatable.Search) > -1)
                .OrderBy(p => p.CategoryId)
                .ToList();
            }

            Datatable.RecordFiltered = categoryProducts.Count();

            //order,paging
            List<Product> productOrdered = OrderProductsByParam(categoryProducts, Datatable.Order, Datatable.OrderDir);
            List<Product> pagingProducts = productOrdered.Skip(Datatable.Start).Take(Datatable.Length).ToList();
            List<ProductDTO> productDTOlist = CreateProductModelList(pagingProducts);

            return Json(
                new {
                    draw = Datatable.Draw,
                    recordsTotal = Datatable.RecordsTotal,
                    recordsFiltered = Datatable.RecordFiltered,
                    data = productDTOlist}
                ,JsonRequestBehavior.AllowGet);
        } 

        //create
        [Route("Create/Product")]
        public ActionResult Create()
        {
            ProductViewModel productViewModel = CreateProductViewModel();
            return View("SaveProduct", productViewModel);
        }


        [HttpPost]
        [Route("Save/Product")]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ProductViewModel productView)
        {
            string url = "/Content/Images/Products/";
            productView.Product.State = true;
            Image image;

            if (productView.Product.ProductId == 0)
            {
                //validaciones
                if (!ModelState.IsValid)
                    return View("SaveProduct", CreateProductViewModel());
                try
                {
                    if (productView.UploadedFile != null)
                    {
                        image = UploadMethod(productView, url);
                        productView.Product.Image = image.RelativePath;
                        productView.UploadedFile.SaveAs(image.AbsolutePath);
                    }
                    else
                        productView.Product.Image = url + "prueba.jpg";

                    dgrosStore.Products.Add(productView.Product);
                }
                catch(Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else
            {
                Product productInDb = GetProduct(productView.Product.ProductId);
                //validacion
                if (!ModelState.IsValid)
                {
                    ProductViewModel editproduct = CreateProductViewModel();
                    editproduct.Product = productInDb;
                    return View("SaveProduct", editproduct);
                }
                //edited product
                productInDb = GetEditedProduct(productInDb,productView);
                if(productView.UploadedFile != null)
                {
                    try
                    {
                        image = UploadMethod(productView, url);
                        if (image.RelativePath != productInDb.Image)
                        {
                            productInDb.Image = image.RelativePath;
                            productView.UploadedFile.SaveAs(image.AbsolutePath);
                        }
                    }catch(Exception ex)
                    {
                        return Content(ex.Message);
                    }         
                }
            }
            try
            {
                dgrosStore.SaveChanges();
                return RedirectToAction("Index", "Product");
            }catch(Exception ex)
            {
                return Content(ex.Message);
            } 
        }

        [Route("Edit/Product/{id}")]
        public ActionResult Edit(int id)
        {
            ProductViewModel productViewModel = CreateProductViewModel();
            Product productInDB = GetProduct(id);

            if (productInDB == null)
                return HttpNotFound();
            else
            {
                productViewModel.Product = productInDB;
                return View("SaveProduct", productViewModel);
            }
            
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            bool state = false;
            Product product = GetProduct(id);
            string error = "";

            if (product == null)
                return Json("0");
            else
            {
                try
                {
                    product.State = state;
                    dgrosStore.SaveChanges();
                    return Json("1");
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                        foreach (var validationError in validationErrors.ValidationErrors)
                            error += String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                    return Json(error);
                }
            }
        }


        protected override void Dispose(bool disposing)
        {
            dgrosStore.Dispose();
        }

        private Image UploadMethod(ProductViewModel productView,string url)
        {
            //para obtener el nombre y la extension del archivo
            string filename = Path.GetFileName(productView.UploadedFile.FileName);
            string RelativePath = url + filename;
            string Absolutepath = Path.Combine(Server.MapPath(url) + filename);

            return new Image() {RelativePath = RelativePath,AbsolutePath = Absolutepath};
        }

        private List<ProductDTO> CreateProductModelList(List<Product> products)
        {
            List<ProductDTO> productDTOList = new List<ProductDTO>();
            foreach(var product in products)
                productDTOList.Add(FillProductDTO(product));

            return productDTOList;
        }

        private List<Product> OrderProductsByParam(List<Product>products,string order,string direction)
        {
            if (!String.IsNullOrWhiteSpace(order) && !String.IsNullOrWhiteSpace(direction))
            {
                if(direction == "asc")
                    products = SwitchStructureAsc(products, order);
                else if(direction == "desc")
                    products = SwitchStructureDesc(products, order);
            }
            return products;
        }

        private ProductViewModel CreateProductViewModel(bool state = true)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Categories = dgrosStore.Categories.Where(p => p.State == state).ToList(),
                Stores = dgrosStore.Stores.ToList(),
                Product = new Product(),
                Providers = dgrosStore.Providers.Where(p => p.State == state).ToList(),
                Discounts = dgrosStore.Discounts.ToList()
            };
            return productViewModel;
        }

        private Product GetEditedProduct(Product product,ProductViewModel productViewModel)
        {
            product.Name = productViewModel.Product.Name;
            product.ShoppingPrice = productViewModel.Product.ShoppingPrice;
            product.SellingPrice = productViewModel.Product.SellingPrice;
            product.CategoryId = productViewModel.Product.CategoryId;
            product.StoreId = productViewModel.Product.StoreId;
            product.Stock = productViewModel.Product.Stock;
            product.MinimunStock = productViewModel.Product.MinimunStock;
            product.Description = productViewModel.Product.Description;
            product.ProviderId = productViewModel.Product.ProviderId;
            return product;
        }

        private Product GetProduct(int id)
        {
            Product product = dgrosStore.Products.SingleOrDefault(p => p.ProductId == id);
            return product;
        }

        private ProductDTO FillProductDTO(Product product)
        {
            ProductDTO productDTO = new ProductDTO()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                SellingPrice = product.SellingPrice,
                ShoppingPrice = product.ShoppingPrice,
                Stock = product.Stock
            };
            return productDTO;
        }

        private List<Product> SwitchStructureAsc(List<Product> products,string order)
        {
            switch (order)
            {
                case "Name":          products = products.OrderBy(p => p.Name).ToList(); return products;
                case "ShoppingPrice": products = products.OrderBy(p => p.ShoppingPrice).ToList(); return products;
                case "SellingPrice":  products = products.OrderBy(p => p.SellingPrice).ToList(); return products;
                case "Stock":         products = products.OrderBy(p => p.Stock).ToList(); return products;
                default:              products = products.ToList(); return products;
            }
        }
        private List<Product> SwitchStructureDesc(List<Product> products, string order)
        {
            switch (order)
            {
                case "Name":          products = products.OrderByDescending(p => p.Name).ToList(); return products;
                case "ShoppingPrice": products = products.OrderByDescending(p => p.ShoppingPrice).ToList(); return products;
                case "SellingPrice":  products = products.OrderByDescending(p => p.SellingPrice).ToList(); return products;
                case "Stock":         products = products.OrderByDescending(p => p.Stock).ToList(); return products;
                default:              products = products.ToList();return products;
            }
        }

        private DataTable CreateDatatable(HttpRequestBase Request)
        {
            DataTable dataTable = new DataTable()
            {
                Draw = Convert.ToInt32(Request["draw"]),
                Start = Convert.ToInt32(Request["start"]),
                Length = Convert.ToInt32(Request["length"]),
                Search = Request["search[value]"],
                Order = Request["columns[" + Request["order[0][column]"] + "][name]"],
                OrderDir = Request["order[0][dir]"],
                RecordsTotal = 0,
                RecordFiltered = 0
            };
            return dataTable;
        }


    }
}