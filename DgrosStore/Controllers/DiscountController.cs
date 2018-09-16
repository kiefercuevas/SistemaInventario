using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgrosStore.Models.viewModels.DiscountDTO;
using DgrosStore.Models;
namespace DgrosStore.Controllers
{
    public class DiscountController : Controller
    {
        private readonly DgrosStoreContext dgrosStore;

        public DiscountController()
        {
            dgrosStore = new DgrosStoreContext();
        }
        // GET: Discount
        public ActionResult Index()
        {
            return View("DiscountIndex");
        }

        public ActionResult GetDiscount()
        {
            List<Discount> discounts = dgrosStore.Discounts.ToList();
            List<DiscountModelDTO> discountModelDTOs = CreateDiscountDTOList(discounts);
            return Json(discountModelDTOs, JsonRequestBehavior.AllowGet);
        }

        [Route("Create/Discount")]
        public ActionResult Create()
        {
            Discount discount = new Discount();
            return View("SaveDiscount",discount);
        }

        

        [Route("Edit/Discount/{id}")]
        public ActionResult Edit(int id)
        {
            Discount discount = GetDiscount(id);
            return View("SaveDiscount", discount);
        }

        [HttpPost]
        public ActionResult Save()
        {
            return Content("working");
        }

        public ActionResult Delete()
        {
            return Content("Delete");
        }


        private List<DiscountModelDTO> CreateDiscountDTOList(List<Discount> discounts)
        {
            List<DiscountModelDTO> discountModelDTOsList = new List<DiscountModelDTO>();

            foreach(Discount distcount in discounts)
            {
                DiscountModelDTO discountModel = new DiscountModelDTO()
                {
                    DiscountId = distcount.DiscountId,
                    DiscountName = distcount.DiscountName,
                    DiscountType = distcount.DiscountType,
                    Discountvalue = distcount.Discountvalue
                };
                discountModelDTOsList.Add(discountModel);
            }
            return discountModelDTOsList;
        }

        private Discount GetDiscount(int id = 0)
        {
            if (id == 0)
                return new Discount();
            else
                return  dgrosStore.Discounts.Where(d => d.DiscountId == id).SingleOrDefault();
        }
    }
}