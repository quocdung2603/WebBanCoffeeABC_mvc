using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;
namespace WebBanCoffeeABC.Areas.Admin.Controllers
{
    public class ManufacturerController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Admin/Manufacturer
        public ActionResult Index(int? page, string SortOrder)
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
            IEnumerable<tHangSX> items = db.tHangSXes.ToList();
            ViewBag.MaHang = SortOrder == "MaHang" ? "MaHang_desc" : "MaHang";
            ViewBag.TenHang = SortOrder == "TenHang" ? "TenHang_desc" : "TenHang";
            switch (SortOrder)
            {
                case "MaHang": items = items.OrderBy(x => x.MaHangSX); break;
                case "MaHang_desc": items = items.OrderByDescending(x => x.MaHangSX);break;
                case "TenHang": items = items.OrderBy(x => x.TenHangSX); break;
                case "TenHang_desc": items = items.OrderByDescending(x => x.TenHangSX); break;
                default:break;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult _NguyenLieuSp(string id)
        {
            var items = db.tNguyenLieuSPs.Where(x => x.MaSP == id);
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection f)
        {
            tHangSX hsx = new tHangSX();
            Random rd = new Random();
            hsx.MaHangSX = "HSX" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            hsx.TenHangSX = f["TenHangSX"];
            db.tHangSXes.Add(hsx);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var hsx = db.tHangSXes.FirstOrDefault(x => x.MaHangSX == id);
            return View(hsx);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection f)
        {
            var mhsx = f["MaHangSX"];
            var hsx = db.tHangSXes.FirstOrDefault(x => x.MaHangSX == mhsx);
            hsx.MaHangSX = mhsx;
            hsx.TenHangSX = f["TenHangSX"];
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hsx = db.tHangSXes.FirstOrDefault(x => x.MaHangSX == id);
            if(hsx != null)
            {
                db.tHangSXes.Remove(hsx);
                db.SaveChanges();
                return Json(new { success = true });
            }    
            return Json(new { success = false });
        }
    }
}