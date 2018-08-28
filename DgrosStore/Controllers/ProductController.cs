using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models;
using System.Data.Entity;
using DgrosStore.Models.viewModels;
using System.IO;
using System.Web.Script.Serialization;
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
            var Products = dgrosStore.Products
                .Where(p => p.State == true)
                .ToList();

            var productModel = CreateProductModelList(Products);
            return Json(productModel, JsonRequestBehavior.AllowGet);
        }
   
        [Route("Product/details/{id}")]
        public ActionResult ProductDetails(int id)
        {
            var product = dgrosStore.Products
                .Include(p => p.Category)
                .SingleOrDefault(p => p.ProductId == id);

            return View(product);
        }

        [Route("Category/{category}")]
        public ActionResult ProductByCategory(string category)
        {
            var categoryProduct = dgrosStore.Products
                .Include(p => p.Category)
                .Where(p => p.Category.Name == category)
                .ToList();


            return View(categoryProduct);
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
            var product = dgrosStore.Products.SingleOrDefault(p => p.ProductId == id);
            if (product == null)
                return Json("0");
            else
            {
                dgrosStore.Products.Remove(product);
                dgrosStore.SaveChanges();
                return Json("1");
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
    }
}