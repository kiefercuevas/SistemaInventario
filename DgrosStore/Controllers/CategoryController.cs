using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models;
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
            var categories = dgrosStore.Categories.ToList();
            return View("CategoryIndex",categories);
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
        public ActionResult Save(Category category)
        {
            if(category.CategoryId == 0)
            {
                if (!ModelState.IsValid)
                {
                    var emptyCategory = new Category();
                    return View("SaveCategory", emptyCategory);
                }
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




        protected override void Dispose(bool disposing)
        {
            dgrosStore.Dispose();
        }
    }
}