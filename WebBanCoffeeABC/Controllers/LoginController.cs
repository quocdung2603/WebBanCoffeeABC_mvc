using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;

namespace WebBanCoffeeABC.Controllers
{
    public class LoginController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var UserName = f["UserName"];
            var Password = f["Password"];
            Session["UserName"] = "";
            Session["Password"] = "";
            tUser user = db.tUsers.SingleOrDefault(n => n.username == UserName && n.password == Password);
            if (user == null)
            {
                TempData["err1"] = "Email or Password is incorrect !!!";
                TempData["UserName"] = UserName;
                TempData["Password"] = Password;

                return Redirect("Login");
            }
            else
            {
                Session["tUser"] = user;
                Session["UserName1"] = UserName;
                Session["Password1"] = Password;
                return RedirectToAction("Index", "ABCHome");
            }

        }
    }
}