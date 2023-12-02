using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;

namespace WebBanCoffeeABC.Areas.Admin.Controllers
{
    public class ProductSizeController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Admin/ProductSize
        public ActionResult Index(string id)
        {
            var items = db.tKichThuocs.Where(x => x.MaSP == id).ToList();
            return View(items);
        }
    }
}