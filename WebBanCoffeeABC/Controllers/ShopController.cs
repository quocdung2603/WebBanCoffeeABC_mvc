using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;

namespace WebBanCoffeeABC.Controllers
{
    public class ShopController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Shop
        public ActionResult Shop()
        {
            return View(db.tDanhMucSPs.ToList());
        }
        [HttpPost]
        public ActionResult Shop(FormCollection f)
        {
            string item = f["search"].ToString();
            var product = db.tDanhMucSPs.Where(n => n.TenSP.Contains(item)).ToList();
            return View(product);
        }
    }
}