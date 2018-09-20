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
using DgrosStore.Models.viewModels.DatatableDTO;

namespace DgrosStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly DgrosStoreContext dgrosStore;
        private readonly bool STATE;
        public ProductController()
        {
            dgrosStore = new DgrosStoreContext();
            STATE = true;
        }

        public ActionResult Index()
        {
            return View("ProductIndex");
        }
        public ActionResult GetProducts()
        {
            List<Product> Products = dgrosStore.Products
                .Where(p => p.State == STATE).OrderBy(p => p.ProductId).ToList();

            DataTable Datatable = CreateDatatableObjectWithRequestValues(Request);
            Datatable.RecordsTotal = Products.Count();

            if (!String.IsNullOrEmpty(Datatable.Search))
                Products = GetProductByParam(Datatable.Search);

            Datatable.RecordFiltered = Products.Count();

            List<ProductDTO> productDTOlist = GetOrderedAndPagedProductDTOList(Products, Datatable);
            DatatableDTO<ProductDTO> datatableDTO = CreateDatatableDTO(productDTOlist, Datatable);

            return Json(datatableDTO, JsonRequestBehavior.AllowGet);
        }
   
        [Route("Product/details/{id}")]
        public ActionResult ProductDetails(int id)
        {
            Product product = dgrosStore.Products
                .Include(p => p.Category)
                .Where(p => p.State == STATE)
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
            List<Product> categoryProducts = dgrosStore.Products
                .Include(p => p.Category)
                .Where(p => p.Category.Name == category)
                .Where(p => p.State == STATE)
                .ToList();

            DataTable Datatable = CreateDatatableObjectWithRequestValues(Request);
            Datatable.RecordsTotal = categoryProducts.Count();

            if (!String.IsNullOrEmpty(Datatable.Search))
                categoryProducts = GetProductAndCategoryByParam(category, Datatable.Search);

            Datatable.RecordFiltered = categoryProducts.Count();
            //order,paging
            List<ProductDTO> productDTOlist = GetOrderedAndPagedProductDTOList(categoryProducts, Datatable);
            DatatableDTO<ProductDTO> datatableDTO = CreateDatatableDTO(productDTOlist,Datatable);

            return Json(datatableDTO, JsonRequestBehavior.AllowGet);
        } 

        
        [Route("Create/Product")]
        public ActionResult Create()
        {
            ProductViewModelDTO productViewModel = CreateProductViewModel();
            return View("SaveProduct", productViewModel);
        }


        [HttpPost]
        [Route("Save/Product")]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ProductViewModelDTO productView)
        {
            string url = "/Content/Images/Products/";
            string defaultImageName = "prueba.jpg";
            Image image;

            productView.Product.State = STATE;

            if (productView.Product.ProductId == 0)
            {
                if (!ModelState.IsValid)
                    return View("SaveProduct", CreateProductViewModel());

                try
                {
                    if (productView.UploadedFile != null)
                    {
                        image = CreateImageObject(productView.UploadedFile.FileName, url);
                        productView.Product = SetImageToProduct(productView.Product, image);
                        productView.UploadedFile.SaveAs(image.AbsolutePath);
                    }
                    else
                        productView.Product.Image = url + defaultImageName;

                    dgrosStore.Products.Add(productView.Product);
                }
                catch(Exception ex){return Content(ex.Message);}
            }
            else
            {
                Product productInDb = GetProduct(productView.Product.ProductId);

                if (productInDb == null)
                    return View("SaveProduct",CreateProductViewModel());

                if (!ModelState.IsValid)
                {
                    ProductViewModelDTO editproduct = CreateProductViewModel();
                    editproduct.Product = productInDb;
                    return View("SaveProduct", editproduct);
                }
                
                productInDb = GetEditedProduct(productInDb,productView);

                if(productView.UploadedFile != null)
                {
                    image = CreateImageObject(productView.UploadedFile.FileName, url);
                    try
                    {
                        productInDb = SetImageToProduct(productInDb, image);
                        productView.UploadedFile.SaveAs(image.AbsolutePath);

                    }
                    catch(Exception ex)
                    {
                        return Content(ex.Message);
                    }         
                }
            }
            try
            {
                dgrosStore.SaveChanges();
                return RedirectToAction("Index", "Product");
            }catch(Exception ex){return Content(ex.Message);} 
        }

        [Route("Edit/Product/{id}")]
        public ActionResult Edit(int id)
        {
            ProductViewModelDTO productViewModel = CreateProductViewModel();
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
            Product product = GetProduct(id);
            string error = "";

            if (product == null)
                return Json("0");
            else
            {
                try
                {
                    product.State = !STATE;
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

        private Image CreateImageObject(string fileName,string url)
        {
            string filename = Path.GetFileName(fileName);
            string RelativePath = url + filename;
            string Absolutepath = Path.Combine(Server.MapPath(url) + filename);

            return new Image() {RelativePath = RelativePath,AbsolutePath = Absolutepath};
        }

        private Product SetImageToProduct(Product product,Image image)
        {
          if (product.ProductId != 0 && product.Image  != image.RelativePath)
              product.Image = image.RelativePath;
          else if(product.ProductId == 0)
              product.Image = image.RelativePath;

            return product;
        }

        private List<ProductDTO> CreateProductDTOList(List<Product> products)
        {
            List<ProductDTO> productDTOList = new List<ProductDTO>();
            foreach(Product product in products)
                productDTOList.Add(CreateProductDTOFromProduct(product));

            return productDTOList;
        }

        private List<Product> OrderProductByDirection(List<Product>products,DataTable dataTable)
        {
            if (!String.IsNullOrWhiteSpace(dataTable.Order) && !String.IsNullOrWhiteSpace(dataTable.OrderDir))
            {
                if(dataTable.OrderDir == "asc")
                    products = OrderProductAscendantByParam(products, dataTable.Order);
                else if(dataTable.OrderDir == "desc")
                    products = OrderProductDescendentByParam(products, dataTable.Order);
            }
            return products;
        }

        private ProductViewModelDTO CreateProductViewModel()
        {
            ProductViewModelDTO productViewModel = new ProductViewModelDTO()
            {
                Categories = dgrosStore.Categories.Where(p => p.State == STATE).ToList(),
                Stores = dgrosStore.Stores.ToList(),
                Product = new Product(),
                Providers = dgrosStore.Providers.Where(p => p.State == STATE).ToList(),
                Discounts = dgrosStore.Discounts.ToList()
            };
            return productViewModel;
        }

        private Product GetEditedProduct(Product product,ProductViewModelDTO productViewModel)
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
            return dgrosStore.Products.SingleOrDefault(p => p.ProductId == id);
        }

        private ProductDTO CreateProductDTOFromProduct(Product product)
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

        private List<Product> OrderProductAscendantByParam(List<Product> products,string order)
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
        private List<Product> OrderProductDescendentByParam(List<Product> products, string order)
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

        private DataTable CreateDatatableObjectWithRequestValues(HttpRequestBase Request)
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

        private List<Product> GetProductByParam(string param)
        {
            List<Product> Products = dgrosStore.Products
                .Where(p => p.State == STATE)
                .Where(p => p.Name.ToLower().IndexOf(param) > -1 ||
                            p.ShoppingPrice.ToString().IndexOf(param) > -1 ||
                            p.SellingPrice.ToString().IndexOf(param) > -1 ||
                            p.Stock.ToString().IndexOf(param) > -1)
                .OrderBy(p => p.CategoryId)
                .ToList();

            return Products;
        }
        private List<Product> GetProductAndCategoryByParam(string category,string param)
        {
            List<Product> categoryProducts = dgrosStore.Products
                    .Include(p => p.Category)
                .Where(p => p.Category.Name == category)
                .Where(p => p.State == STATE)
                .Where(p => p.Name.ToLower().IndexOf(param) > -1 ||
                            p.ShoppingPrice.ToString().IndexOf(param) > -1 ||
                            p.SellingPrice.ToString().IndexOf(param) > -1 ||
                            p.Stock.ToString().IndexOf(param) > -1)
                .OrderBy(p => p.CategoryId)
                .ToList();
            return categoryProducts;
        }
        private DatatableDTO<ProductDTO> CreateDatatableDTO(List<ProductDTO> products,DataTable datatable)
        {
            DatatableDTO<ProductDTO> datatableDTO = new DatatableDTO<ProductDTO>()
            {
                draw = datatable.Draw,
                recordsTotal = datatable.RecordsTotal,
                recordsFiltered = datatable.RecordFiltered,
                data = products
            };
            return datatableDTO;
        }

        private List<Product> GetPagingProduct(List<Product> products,DataTable dataTable)
        {
            return products.Skip(dataTable.Start).Take(dataTable.Length).ToList();
        }
        private List<ProductDTO> GetOrderedAndPagedProductDTOList(List<Product> products,DataTable dataTable)
        {
            products = OrderProductByDirection(products, dataTable);
            products = GetPagingProduct(products, dataTable);

            return CreateProductDTOList(products);
        }
    }
}