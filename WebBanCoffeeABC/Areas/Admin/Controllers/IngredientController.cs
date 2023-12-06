using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;
using PagedList;
using PagedList.Mvc;
namespace WebBanCoffeeABC.Areas.Admin.Controllers
{
    public class IngredientController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Admin/Ingredient
        public ActionResult Index(int ? page, string SortOrder)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<tNguyenLieu> items = db.tNguyenLieux.ToList();
            ViewBag.MaNguyenLieu = SortOrder == "MaNguyenLieu" ? "MaNguyenLieu_desc" : "MaNguyenLieu";
            ViewBag.TenNguyenLieu = SortOrder == "TenNguyenLieu" ? "TenNguyenLieu_desc" : "TenNguyenLieu";
            switch (SortOrder)
            {
                case "MaNguyenLieu":
                    items = items.OrderBy(x => x.MaNguyenLieu);
                    break;
                case "MaNguyenLieu_desc":
                    items = items.OrderByDescending(x => x.TenNguyenLieu);
                    break;
                case "TenNguyenLieu":
                    items = items.OrderBy(x => x.TenNguyenLieu);
                    break;
                case "TenNguyenLieu_desc":
                    items = items.OrderByDescending(x => x.TenNguyenLieu);
                    break;
                default: break;
            }    
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection f)
        {
            tNguyenLieu nl = new tNguyenLieu();
            Random rd = new Random();
            nl.MaNguyenLieu = "NL" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            nl.TenNguyenLieu = f["TenNguyenLieu"];
            nl.GiaMua = Convert.ToDecimal(f["GiaMua"]);
            nl.ConLai = Convert.ToDecimal(f["ConLai"]);
            nl.DonViTinh = f["DonViTinh"];
            db.tNguyenLieux.Add(nl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var item = db.tNguyenLieux.FirstOrDefault(x => x.MaNguyenLieu == id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection f)
        {
            var mnl = f["MaNguyenLieu"];
            var nl = db.tNguyenLieux.FirstOrDefault(x => x.MaNguyenLieu == mnl);
            nl.TenNguyenLieu = f["TenNguyenLieu"];
            nl.GiaMua = Convert.ToDecimal(f["GiaMua"]);
            nl.ConLai = Convert.ToDecimal(f["ConLai"]);
            nl.DonViTinh = f["DonViTinh"];
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult Detail(string id)
        {
            var item = db.tNguyenLieux.FirstOrDefault(x => x.MaNguyenLieu == id);
            return View(item);
        }
        
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var item = db.tNguyenLieux.FirstOrDefault(x => x.MaNguyenLieu == id);
            if(item!=null)
            {
                db.tNguyenLieux.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}