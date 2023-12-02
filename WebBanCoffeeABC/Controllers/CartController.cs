using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;

namespace WebBanCoffeeABC.Controllers
{
    public class CartController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewCard()
        {
            ViewBag.Number = TotalNumberProduct();
            return PartialView();
        }
        private int TotalNumberProduct()
        {
            int totalNumberProduct = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                totalNumberProduct = lstCart.Sum(n => n.ProductNumber);
            }
            return totalNumberProduct;
        }
    }
}