using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;

namespace WebBanCoffeeABC.Controllers
{
    public class ABCHomeController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: ABCHome
        public ActionResult Index()
        {
            var p = db.tDanhMucSPs.ToList();
            return View(p);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult ProductDetails()
        {
            return View();
        }
        public ActionResult TypeCoffee()
        {
            return PartialView(db.tLoaiSPs.ToList());
        }
        public ActionResult ShowP(string id)
        {
            var item = db.tDanhMucSPs.Where(x => x.MaLoai == id);
            return PartialView(item);
        }
    }
}