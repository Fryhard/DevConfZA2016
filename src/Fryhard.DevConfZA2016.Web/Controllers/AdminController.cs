using Fryhard.DevConfZA2016.Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fryhard.DevConfZA2016.Web.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Stop()
        {
            Control control = new Control()
            {
                Stop = true
            };

            BusHost.Publish<Control>(control, BusTopic.Control);
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Start()
        {
            Control control = new Control()
            {
                Start = true
            };

            BusHost.Publish<Control>(control, BusTopic.Control);
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Reset()
        {
            Control control = new Control()
            {
                Reset = true
            };

            BusHost.Publish<Control>(control, BusTopic.Control);
            return RedirectToAction("Index", "Admin");
        }
    }
}
