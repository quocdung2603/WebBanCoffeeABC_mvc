using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;
namespace WebBanCoffeeABC.Areas.Admin.Controllers
{
    public class ProductTypeController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Admin/ProductType
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var items = db.tLoaiSPs.ToList();
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection f)
        {
            tLoaiSP lsp = new tLoaiSP();
            Random rd = new Random();
            lsp.MaLoai = "LSP" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            lsp.Loai = f["Loai"];
            db.tLoaiSPs.Add(lsp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var lsp = db.tLoaiSPs.FirstOrDefault(x => x.MaLoai == id);
            return View(lsp);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection f)
        {
            var ml = f["MaLoai"];
            var lsp = db.tLoaiSPs.FirstOrDefault(x => x.MaLoai == ml);
            lsp.MaLoai = ml;
            lsp.Loai = f["Loai"];
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]  
        public ActionResult Delete(string  id)
        {
            var lsp = db.tLoaiSPs.FirstOrDefault(x => x.MaLoai == id);
            if(lsp != null)
            {
                db.tLoaiSPs.Remove(lsp);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}