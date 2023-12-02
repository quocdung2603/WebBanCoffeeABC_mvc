using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;
namespace WebBanCoffeeABC.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Admin/User
        public ActionResult Index_Customer()
        {
            var items = db.tKhachHangs.ToList();
            return View(items);
        }
        public ActionResult Index_Staff()
        {
            var items = db.tNhanViens.ToList();
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection f)
        {
            tUser u = new tUser();
            u.username = f["username"];
            u.password = f["password"];
            u.LoaiUser = 1; // 1 - nhân viên, 0 - khách hàng
            db.tUsers.Add(u);
            db.SaveChanges();
            tNhanVien nv = new tNhanVien();
            Random rd = new Random();
            nv.MaNhanVien = "NV" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            nv.TenNhanVien = f["TenNhanVien"];
            nv.NgaySinh = Convert.ToDateTime(f["NgaySinh"]);
            nv.SoDienThoai = f["SoDienThoai"];
            nv.DiaChi = f["DiaChi"];
            nv.AnhDaiDien = f["AnhDaiDien"];
            nv.ChucVu = f["ChucVu"];
            nv.GhiChu = f["GhiChu"];
            nv.username = f["username"];
            db.tNhanViens.Add(nv);
            db.SaveChanges();
            return RedirectToAction("Index_Staff");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var nv = db.tNhanViens.FirstOrDefault(x => x.MaNhanVien == id);
            if (nv != null)
            {
                var u = db.tUsers.FirstOrDefault(x => x.username == nv.username);
                db.tUsers.Remove(u);
                db.tNhanViens.Remove(nv);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public ActionResult Detail(string id)
        {
            var nv = db.tNhanViens.FirstOrDefault(x => x.MaNhanVien == id);  
            return View(nv);
        }

        public ActionResult Detail_Customer(string id)
        {
            var kh = db.tKhachHangs.FirstOrDefault(x => x.MaKhanhHang == id);
            return View(kh);
        }

        public ActionResult Edit(string id)
        {
            var nv = db.tNhanViens.FirstOrDefault(x => x.MaNhanVien == id);
            return View(nv);
        }
        
        [HttpPost]
        public ActionResult Edit(FormCollection f)
        {
            var usn = f["username"];
            var u = db.tUsers.FirstOrDefault(x => x.username == usn);
            u.username = usn;
            u.password = f["password"];
            db.SaveChanges();
            var mnv = f["MaNhanVien"];
            var nv = db.tNhanViens.FirstOrDefault(x => x.MaNhanVien == mnv);
            nv.TenNhanVien = f["TenNhanVien"];
            nv.NgaySinh = Convert.ToDateTime(f["NgaySinh"]);
            nv.SoDienThoai = f["SoDienThoai"];
            nv.DiaChi = f["DiaChi"];
            nv.ChucVu = f["ChucVu"];
            nv.AnhDaiDien = f["AnhDaiDien"];
            nv.GhiChu = f["GhiChu"];
            db.SaveChanges();
            return RedirectToAction("Index_Staff");
        }
    }
}