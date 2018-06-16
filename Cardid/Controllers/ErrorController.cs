using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cardid.Controllers
{
    [HandleError]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                ViewBag.LoggedOut = true;
            }
            return View("Default");
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}