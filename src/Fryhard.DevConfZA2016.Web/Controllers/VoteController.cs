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

            HttpCookie cookie = Request.Cookies.Get("Voter");
            if (cookie == null)
            {
                //Drop a cookie on the visitor's machine so we can remember them.
                cookie = new HttpCookie("Voter");
                cookie.Value = Guid.NewGuid().ToString();
                cookie.Expires = DateTime.Now.AddDays(1); // We only need to store this cookie for 1 day
                Response.Cookies.Add(cookie);
            }

            return View();
        }
    }
}
