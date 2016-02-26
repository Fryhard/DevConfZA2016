using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fryhard.DevConfZA2016.Web.Controllers
{
    public class VoteController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CastVote(int number)
        {

            return null;

        }

    }
}
