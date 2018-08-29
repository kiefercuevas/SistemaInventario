using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models;
using DgrosStore.Models.viewModels;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace DgrosStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DgrosStoreContext dgrosStore;

        public CategoryController()
        {
            dgrosStore = new DgrosStoreContext();
        }
        // GET: Category
        public ActionResult Index()
        {
            return View("CategoryIndex");
        }

        public ActionResult GetCategories()
        {

            var categories = dgrosStore.Categories
                .Where(c => c.State == true)
                .ToList();
            var categoryModel = CreateCategoryModelList(categories);

            return Json(categoryModel, JsonRequestBehavior.AllowGet);
        }
        
        [Route("Create/Category")]
        public ActionResult Create()
        {
            var category = new Category();
            return View("saveCategory",category);
        }

        //add category
        [HttpPost]
        [Route("Save/Category")]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Category category)
        {
            var state = true;
            if(category.CategoryId == 0)
            {
                if (!ModelState.IsValid)
                {
                    var emptyCategory = new Category();
                    return View("SaveCategory", emptyCategory);
                }
                category.State = state;
                dgrosStore.Categories.Add(category);
            }
                
            else
            {
                var categoryInDB = dgrosStore.Categories.SingleOrDefault(c => c.CategoryId == category.CategoryId);
                if (!ModelState.IsValid)
                {
                    var editCategory = categoryInDB;
                    return View("SaveCategory", editCategory);
                }

                categoryInDB.Name = category.Name;
            }
            
            dgrosStore.SaveChanges();
            return RedirectToAction("Index","Category");
        }

        [Route("Edit/Category/{id}")]
        public ActionResult Edit(int id)
        {
            var Category = dgrosStore.Categories.SingleOrDefault(c => c.CategoryId == id);
            if(Category == null)
                return HttpNotFound();
            else
                return View("SaveCategory",Category);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var state = false;
            var error = "";
            var category = dgrosStore.Categories
                .Include(c => c.Products)
                .SingleOrDefault(c => c.CategoryId == id);
                

            if (category == null)
                return Json("0");
            else
            {
                try
                {
                    category.State = state;
                    foreach (var product in category.Products)
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

        private List<CategoryModel> CreateCategoryModelList(List<Category> categories)
        {
            var categoryModelList = new List<CategoryModel>();
            foreach (var category in categories)
            {
                var categoryModel = new CategoryModel()
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name
                };
                categoryModelList.Add(categoryModel);
            }

            return categoryModelList;
        }

        

        protected override void Dispose(bool disposing)
        {
            dgrosStore.Dispose();
        }
    }
}