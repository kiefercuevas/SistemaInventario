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
            var state = true;
            var Products = dgrosStore.Products.Where(p => p.State == state).OrderBy(p => p.ProductId).ToList();
            var Datatable = new DataTable()
            {
                Draw = Convert.ToInt32(Request["draw"]),
                Start = Convert.ToInt32(Request["start"]),
                Length = Convert.ToInt32(Request["length"]),
                Search = Request["search[value]"],
                Order = Request["columns[" + Request["order[0][column]"] + "][name]"],
                OrderDir = Request["order[0][dir]"],
                RecordsTotal = Products.Count(),
                RecordFiltered = 0
            };

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
            var productOrdered = OrderProductsByParam(Products, Datatable.Order, Datatable.OrderDir);
            var pagingProducts = productOrdered.Skip(Datatable.Start).Take(Datatable.Length).ToList();
            var productModelList = CreateProductModelList(pagingProducts);

            var ProductsModelToDataTable = new
            {
                draw = Datatable.Draw,
                recordsTotal = Datatable.RecordsTotal,
                recordsFiltered = Datatable.RecordFiltered,
                data = productModelList,
            };

            return Json(ProductsModelToDataTable, JsonRequestBehavior.AllowGet);
        }
   
        [Route("Product/details/{id}")]
        public ActionResult ProductDetails(int id)
        {
            var state = true;
            var product = dgrosStore.Products
                .Include(p => p.Category)
                .Where(p => p.State == state)
                .SingleOrDefault(p => p.ProductId == id);

            if (product == null)
                return HttpNotFound();
            else
                return View(product);
        }

        [Route("Product/Category/{category}")]
        public ActionResult ProductByCategory()
        {
            return View();
        }

        public ActionResult GetProductByCategory(string category)
        {
            var state = true;
            var categoryProducts = dgrosStore.Products
                .Include(p => p.Category)
                .Where(p => p.Category.Name == category)
                .Where(p => p.State == state)
                .ToList();

            var Datatable = new DataTable()
            {
                Draw = Convert.ToInt32(Request["draw"]),
                Start = Convert.ToInt32(Request["start"]),
                Length = Convert.ToInt32(Request["length"]),
                Search = Request["search[value]"],
                Order = Request["columns[" + Request["order[0][column]"] + "][name]"],
                OrderDir = Request["order[0][dir]"],
                RecordsTotal = categoryProducts.Count(),
                RecordFiltered = 0
            };

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
            var productOrdered = OrderProductsByParam(categoryProducts, Datatable.Order, Datatable.OrderDir);
            var pagingProducts = productOrdered.Skip(Datatable.Start).Take(Datatable.Length).ToList();
            var productModelList = CreateProductModelList(pagingProducts);


            var ProductsModelToDataTable = new
            {
                draw = Datatable.Draw,
                recordsTotal = Datatable.RecordsTotal,
                recordsFiltered = Datatable.RecordFiltered,
                data = productModelList,
            };

            return Json(ProductsModelToDataTable, JsonRequestBehavior.AllowGet);
        } 

        //create
        [Route("Create/Product")]
        public ActionResult Create()
        {
            var categories = dgrosStore.Categories.ToList();
            var stores = dgrosStore.Stores.ToList();
            var product = new ProductViewModel()
            {
                Categories = categories,
                Stores = stores,
                Product = new Product()
            };
            return View("SaveProduct",product);
        }


        [HttpPost]
        [Route("Save/Product")]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ProductViewModel productView)
        {
            string url = "/Content/Images/Products/";
            productView.Product.State = true;

            if (productView.Product.ProductId == 0)
            {
                //validaciones
                if (!ModelState.IsValid)
                {
                    var emptyproduct = new ProductViewModel()
                    {
                        Categories = dgrosStore.Categories.ToList(),
                        Stores = dgrosStore.Stores.ToList(),
                        Product = new Product()
                    };
                    return View("SaveProduct", emptyproduct);
                }


                try
                {
                    if (productView.UploadedFile != null)
                    {
                        Image data = UploadMethod(productView, url);
                        productView.Product.Image = data.ImagePath;
                        productView.UploadedFile.SaveAs(data.CompletePath);
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
                //validacion
                if (!ModelState.IsValid)
                {
                    var categories = dgrosStore.Categories.ToList();
                    var stores = dgrosStore.Stores.ToList();

                    var productInDb = dgrosStore.Products.SingleOrDefault(p => p.ProductId == productView.Product.ProductId);
                    var editproduct = new ProductViewModel()
                    {
                        Categories = categories,
                        Stores = stores,
                        Product = productInDb
                    };
                    return View("SaveProduct", editproduct);
                }


                var productInDB = dgrosStore.Products.SingleOrDefault(p => p.ProductId == productView.Product.ProductId);

                productInDB.Name = productView.Product.Name;
                productInDB.ShoppingPrice = productView.Product.ShoppingPrice;
                productInDB.SellingPrice = productView.Product.SellingPrice;
                productInDB.CategoryId = productView.Product.CategoryId;
                productInDB.StoreId = productView.Product.StoreId;
                productInDB.Stock = productView.Product.Stock;
                productInDB.MinimunStock = productView.Product.MinimunStock;
                productInDB.Description = productView.Product.Description;

                if(productView.UploadedFile != null)
                {
                    try
                    {
                        Image data = UploadMethod(productView, url);
                        if (data.ImagePath != productInDB.Image)
                        {
                            productInDB.Image = data.ImagePath;
                            productView.UploadedFile.SaveAs(data.CompletePath);
                        }
                    }catch(Exception ex)
                    {
                        return Content(ex.Message);
                    }         
                }
            }
            dgrosStore.SaveChanges();

            return RedirectToAction("Index", "Product");
        }

        [Route("Edit/Product/{id}")]
        public ActionResult Edit(int id)
        {
            var categories = dgrosStore.Categories.ToList();
            var stores = dgrosStore.Stores.ToList();
            var productInDB = dgrosStore.Products.SingleOrDefault(p => p.ProductId == id);

            if (productInDB == null)
                return HttpNotFound();
            else
            {
                var product = new ProductViewModel()
                {
                    Categories = categories,
                    Stores = stores,
                    Product = productInDB
                };
                return View("SaveProduct", product);
            }
            
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var state = false;
            var product = dgrosStore.Products.SingleOrDefault(p => p.ProductId == id);
            var error = "";
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


        protected override void Dispose(bool disposing)
        {
            dgrosStore.Dispose();
        }

        private Image UploadMethod(ProductViewModel productView,string url)
        {
            //para obtener el nombre y la extension del archivo
            string filename = Path.GetFileName(productView.UploadedFile.FileName);
            var path = Path.Combine(Server.MapPath(url) + filename);

            return new Image() {
                //imagePath es la ruta relativa
                ImagePath = url+filename,
                //ruta absoluta
                CompletePath = path
            };
        }

        private List<GetProductModel> CreateProductModelList(List<Product> products)
        {
            var productModelList = new List<GetProductModel>();
            foreach(var product in products)
            {
                var productModel = new GetProductModel()
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    SellingPrice = product.SellingPrice,
                    ShoppingPrice = product.ShoppingPrice,
                    Stock = product.Stock
                };
                productModelList.Add(productModel);
            }

            return productModelList;
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





    }
}