using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DgrosStore.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        [Route("Error/404/13")]
        public ActionResult Error40413()
        {
            return View("Error40413");
        }
    }
}