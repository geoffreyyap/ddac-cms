using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["PersonRole"] = "";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Maersk Line Container Management System";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Maersk Line Contact";

            return View();
        }
    }
}