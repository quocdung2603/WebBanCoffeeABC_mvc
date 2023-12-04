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
        public ActionResult Index(FormCollection f)
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

        public ActionResult ProductDetails(string MaSP)
        {
            tDanhMucSP product = db.tDanhMucSPs.FirstOrDefault(n => n.MaSP == MaSP);
            return View(product);
        }
        public ActionResult TypeCoffee()
        {
            return PartialView(db.tLoaiSPs.ToList());
        }

        [HttpGet]
        public ActionResult ShowP(string id)
        {
            var item = db.tDanhMucSPs.Where(x => x.MaLoai == id);
            return PartialView(item);
        }
       
    }
}