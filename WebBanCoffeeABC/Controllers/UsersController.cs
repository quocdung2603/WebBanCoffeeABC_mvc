using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;

namespace WebBanCoffeeABC.Controllers
{
    public class UsersController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["tUser"] = null;
            return RedirectToAction("Index", "ABCHome");
        }
        public ActionResult Information(int id)
        {

            return View();
        }
    }
}