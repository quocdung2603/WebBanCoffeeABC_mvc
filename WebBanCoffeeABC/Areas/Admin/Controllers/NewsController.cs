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
    public class NewsController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Admin/News
        public ActionResult Index(int ?page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<tTinTuc> items = db.tTinTucs.OrderByDescending(x=>x.NgayTao);
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(FormCollection f)
        {
            tTinTuc n = new tTinTuc();
            n.TenTinTuc = f["TenTinTuc"];
            n.MoTa = f["MoTa"];
            n.NoiDung = f["NoiDung"];
            n.AnhDaiDien = f["AnhDaiDien"];
            n.NgayTao = DateTime.Now;
            n.NgayChinhSua = DateTime.Now;
            db.tTinTucs.Add(n);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var item = db.tTinTucs.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        public ActionResult Edit(int id)
        {
            var item = db.tTinTucs.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var id = Convert.ToInt32(f["Id"]);
            var n = db.tTinTucs.FirstOrDefault(x => x.Id == id);
            n.TenTinTuc = f["TenTinTuc"];
            n.MoTa = f["MoTa"];
            n.NoiDung = f["NoiDung"];
            n.AnhDaiDien = f["AnhDaiDien"];
            n.NgayChinhSua = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.tTinTucs.FirstOrDefault(x => x.Id == id);
            db.tTinTucs.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}