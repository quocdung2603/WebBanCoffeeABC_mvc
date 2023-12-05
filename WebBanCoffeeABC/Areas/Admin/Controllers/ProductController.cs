using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;
using PagedList;
using PagedList.Mvc;
namespace WebBanCoffeeABC.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Admin/Product
        public ActionResult Index(int?page, string SortOrder)
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
            ViewBag.MaSP = SortOrder == "MaSP" ? "MaSP_desc" : "MaSP";
            ViewBag.TenSP = SortOrder == "TenSP" ? "TenSP_desc" : "TenSP";
            IEnumerable<tDanhMucSP> items = db.tDanhMucSPs.ToList();
            switch (SortOrder)
            {
                case "MaSP":
                    items = items.OrderBy(x => x.MaSP);
                    break;
                case "MaSP_desc":
                    items = items.OrderByDescending(x => x.MaSP);
                    break;
                case "TenSP":
                    items = items.OrderBy(x => x.TenSP);
                    break;
                case "TenSP_desc":
                    items = items.OrderByDescending(x => x.TenSP);
                    break;
                default:
                    break;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        
        public ActionResult Add()
        {
            ViewBag.ProductType = new SelectList(db.tLoaiSPs.ToList(), "MaLoai", "Loai"); 
            ViewBag.Manufacturer = new SelectList(db.tHangSXes.ToList(), "MaHangSX", "TenHangSX");
            ViewBag.Ingredient = db.tNguyenLieux.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection f , List<string> Images, List<int> rDefault, List<tKichThuoc> LProduct, List<tNguyenLieuSP> IP)
        {
            tDanhMucSP p = new tDanhMucSP();
            Random rd = new Random();
            p.MaSP = "SP" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            p.TenSP = f["TenSP"];
            p.MaLoai = f["MaLoai"];
            p.GioiThieuSP = f["GioiThieuSP"];
            p.MaHangSX = f["MaHangSX"];
            p.GiaGoc = Convert.ToDecimal(f["GiaGoc"]);
            db.tDanhMucSPs.Add(p);
            db.SaveChanges();

            if (Images != null && Images.Count > 0)
            {
                for (int i = 0; i < Images.Count; i++)
                {
                    if (i + 1 == rDefault[0])
                    {
                        p.tAnhSPs.Add(new tAnhSP
                        {
                            MaSP = p.MaSP,
                            TenFileAnh = Images[i],
                            AnhMacDinh = true,
                        });
                    }
                    else
                    {
                        p.tAnhSPs.Add(new tAnhSP
                        {
                            MaSP = p.MaSP,
                            TenFileAnh = Images[i],
                            AnhMacDinh = false,
                        });
                    }
                }
            }

            for (int i = 0; i < LProduct.Count; i++)
            {
                var sz = f["LProduct[" + i + "].KichThuoc"];
                var pr = Convert.ToDecimal(f["LProduct[" + i + "].GiaBan"]);
                var prs = Convert.ToDecimal(f["LProduct[" + i + "].GiaKhuyenMai"]);
                p.tKichThuocs.Add(new tKichThuoc
                {
                    MaKichThuoc = "KT" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9),
                    KichThuoc = sz,
                    MaSP = p.MaSP,
                    GiaBan = pr,
                    GiaKhuyenMai = prs,
                });
            }

            for(int i =0;i< IP.Count;i++)
            {
                var mnl = f["IP[" + i + "].MaNguyenLieu"];
                var l = Convert.ToDecimal(f["IP[" + i + "].Luong"]);
                p.tNguyenLieuSPs.Add(new tNguyenLieuSP
                {
                    MaNguyenLieu = mnl,
                    MaSP = p.MaSP,
                    Luong = l,
                });
            }   
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            var item = db.tDanhMucSPs.FirstOrDefault(x => x.MaSP == id);
            return View(item);
        }
         
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var p = db.tDanhMucSPs.FirstOrDefault(x => x.MaSP == id);
            if(p !=null)
            {
                var imgP = db.tAnhSPs.Where(x => x.MaSP == p.MaSP);
                var szP = db.tKichThuocs.Where(x => x.MaSP == p.MaSP);
                var iP = db.tNguyenLieuSPs.Where(x => x.MaSP == p.MaSP);
                foreach(var item in imgP)
                {
                    db.tAnhSPs.Remove(item);
                }
                foreach(var item in szP)
                {
                    db.tKichThuocs.Remove(item);
                }
                foreach(var item in iP)
                {
                    db.tNguyenLieuSPs.Remove(item);
                }
                db.tDanhMucSPs.Remove(p);
                db.SaveChanges();
                return Json(new { success = true });
            }    
            return Json(new { success = false });
        }
        
        public ActionResult Edit(string id)
        {
            var item = db.tDanhMucSPs.FirstOrDefault(x => x.MaSP == id);
            ViewBag.ProductType = new SelectList(db.tLoaiSPs.ToList(), "MaLoai", "Loai");
            ViewBag.Manufacturer = new SelectList(db.tHangSXes.ToList(), "MaHangSX", "TenHangSX");
            ViewBag.Ingredient = db.tNguyenLieux.ToList();
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection f, List<string> Images, List<int> rDefault, List<tKichThuoc> LProduct, List<tNguyenLieuSP> IP)
        {
            Random rd = new Random();
            var _id = f["MaSP"];
            var p = db.tDanhMucSPs.FirstOrDefault(x=>x.MaSP == _id);
            p.TenSP = f["TenSP"];
            p.MaLoai = f["MaLoai"];
            p.GioiThieuSP = f["GioiThieuSP"];
            p.MaHangSX = f["MaHangSX"];
            p.GiaGoc = Convert.ToDecimal(f["GiaGoc"]);
            var imgP = db.tAnhSPs.Where(x => x.MaSP == p.MaSP);
            var szP = db.tKichThuocs.Where(x => x.MaSP == p.MaSP);
            var iP = db.tNguyenLieuSPs.Where(x => x.MaSP == p.MaSP);
            foreach (var item in imgP)
            {
                db.tAnhSPs.Remove(item);
            }
            foreach (var item in szP)
            {
                db.tKichThuocs.Remove(item);
            }
            foreach (var item in iP)
            {
                db.tNguyenLieuSPs.Remove(item);
            }
            if (Images != null && Images.Count > 0)
            {
                for (int i = 0; i < Images.Count; i++)
                {
                    if (i  == rDefault[0])
                    {
                        p.tAnhSPs.Add(new tAnhSP
                        {
                            MaSP = p.MaSP,
                            TenFileAnh = Images[i],
                            AnhMacDinh = true,
                        });
                    }
                    else
                    {
                        p.tAnhSPs.Add(new tAnhSP
                        {
                            MaSP = p.MaSP,
                            TenFileAnh = Images[i],
                            AnhMacDinh = false,
                        });
                    }
                }
            }

            for (int i = 0; i < LProduct.Count; i++)
            {
                var sz = f["LProduct[" + i + "].KichThuoc"];
                var pr = Convert.ToDecimal(f["LProduct[" + i + "].GiaBan"]);
                var prs = Convert.ToDecimal(f["LProduct[" + i + "].GiaKhuyenMai"]);
                p.tKichThuocs.Add(new tKichThuoc
                {
                    MaKichThuoc = "KT" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9),
                    KichThuoc = sz,
                    MaSP = p.MaSP,
                    GiaBan = pr,
                    GiaKhuyenMai = prs,
                });
            }

            for (int i = 0; i < IP.Count; i++)
            {
                var mnl = f["IP[" + i + "].MaNguyenLieu"];
                var l = Convert.ToDecimal(f["IP[" + i + "].Luong"]);
                p.tNguyenLieuSPs.Add(new tNguyenLieuSP
                {
                    MaNguyenLieu = mnl,
                    MaSP = p.MaSP,
                    Luong = l,
                });
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}