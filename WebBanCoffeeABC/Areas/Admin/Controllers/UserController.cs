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
    public class UserController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Admin/User
        public ActionResult Index_Customer(int ? page, string SortOrder)
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
            IEnumerable<tKhachHang> items = db.tKhachHangs.ToList();
            ViewBag.MaU = SortOrder == "MaU" ? "MaUd" : "MaU";
            ViewBag.TenU = SortOrder == "TenU" ? "TenUd" : "TenU";
            ViewBag.Loai = SortOrder == "Loai" ? "Loaid" : "Loai";
            switch(SortOrder)
            {
                case "MaU": items = items.OrderBy(x => x.MaKhachHang);break;
                case "MaUd": items = items.OrderByDescending(x => x.MaKhachHang);break;
                case "TenU": items = items.OrderBy(x => x.TenKhachHang); break;
                case "TenUd": items = items.OrderByDescending(x => x.TenKhachHang); break;
                case "Loai": items = items.OrderBy(x => x.LoaiKhachHang); break;
                case "Loaid": items = items.OrderByDescending(x => x.LoaiKhachHang); break;
                default:break;
            }    
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Index_Staff(int? page, string SortOrder)
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
            IEnumerable<tNhanVien> items = db.tNhanViens.ToList();
            ViewBag.MaU = SortOrder == "MaU" ? "MaUd" : "MaU";
            ViewBag.TenU = SortOrder == "TenU" ? "TenUd" : "TenU";
            ViewBag.Loai = SortOrder == "Loai" ? "Loaid" : "Loai";
            switch (SortOrder)
            {
                case "MaU": items = items.OrderBy(x => x.MaNhanVien); break;
                case "MaUd": items = items.OrderByDescending(x => x.MaNhanVien); break;
                case "TenU": items = items.OrderBy(x => x.TenNhanVien); break;
                case "TenUd": items = items.OrderByDescending(x => x.TenNhanVien); break;
                case "Loai": items = items.OrderBy(x => x.ChucVu); break;
                case "Loaid": items = items.OrderByDescending(x => x.ChucVu); break;
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
            var kh = db.tKhachHangs.FirstOrDefault(x => x.MaKhachHang == id);
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


        //Login  - Logout
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var UserName = f["UserName"];
            var Password = f["Password"];
            Session["Admin"] = new tUser();
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không được để trống";
                Redirect("Login");
            }
            tUser u = db.tUsers.FirstOrDefault(x => x.username == UserName && x.password == Password);
            if (u != null)
            {
                var nv = db.tNhanViens.FirstOrDefault(x => x.username == u.username);
                if(nv != null)
                {
                    Session["Admin"] = u;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Tài Khoản hoặc mật khẩu không đúng";
                    return Redirect("Login");
                }
            }
            else
            {
                ViewBag.Error = "Tài Khoản hoặc mật khẩu không đúng";
                return Redirect("Login");
            }
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login","User");
        }
    }
}