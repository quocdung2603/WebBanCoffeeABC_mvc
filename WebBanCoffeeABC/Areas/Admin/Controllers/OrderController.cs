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
    public class OrderController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<tHoaDonBan> items = db.tHoaDonBans.ToList();
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
        public ActionResult Add(FormCollection f, List<tChiTietHDB> LProduct)
        {
            tHoaDonBan o = new tHoaDonBan();
            Random rd = new Random();
            o.MaHoaDon = "HD" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            o.GhiChu = f["GhiChu"];
            o.MaKhachHang = f["MaKhachHang"];
            /*o.MaNhanVien = "";*/ //chưa làm đăng nhập cho admin
            o.NgayHoaDon = DateTime.Now;
            o.TongTienHD = Convert.ToDecimal(f["TongTienHD"]);
            o.GiamGiaHD = Convert.ToDouble(f["GiamGiaHD"]);
            o.PhuongThucThanhToan = Convert.ToByte(f["PhuongThucThanhToan"]);
            for(var i =0; i< LProduct.Count; i++)
            {
                var sz = f["LProduct[" + i + "].KichCoSP"];
                var sl = Convert.ToInt32(f["LProduct[" + i + "].SoLuongSP"]);
                var gb = Convert.ToDecimal(f["LProduct[" + i + "].GiaBanSP"]);
                var msp = f["LProduct[" + i + "].MaSP"];
                o.tChiTietHDBs.Add(new tChiTietHDB { 
                    MaHoaDon = o.MaHoaDon,
                    MaSP = msp,
                    KichCo = sz,
                    GiaBan = gb,
                    SoLuong= sl,
                });
            }
            db.tHoaDonBans.Add(o);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(string id)
        {
            var item = db.tHoaDonBans.FirstOrDefault(x => x.MaHoaDon == id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var item = db.tHoaDonBans.FirstOrDefault(x => x.MaHoaDon == id);
            var od = db.tChiTietHDBs.Where(x => x.MaHoaDon == item.MaHoaDon);
            foreach(var it  in od)
            {
                db.tChiTietHDBs.Remove(it);
            }    
            db.tHoaDonBans.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }

        public ActionResult Edit(string id)
        {
            var item = db.tHoaDonBans.FirstOrDefault(x => x.MaHoaDon == id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection f, List<tChiTietHDB> LProduct)
        {
            var mhd = f["MaHoaDon"];
            var o = db.tHoaDonBans.FirstOrDefault(x => x.MaHoaDon == mhd);
            o.GhiChu = f["GhiChu"];
            o.MaKhachHang = f["MaKhachHang"];
            /*o.MaNhanVien = "";*/ //chưa làm đăng nhập cho admin
            o.TongTienHD = Convert.ToDecimal(f["TongTienHD"]);
            o.GiamGiaHD = Convert.ToDouble(f["GiamGiaHD"]);
            o.PhuongThucThanhToan = Convert.ToByte(f["PhuongThucThanhToan"]);
            var ods = db.tChiTietHDBs.Where(x => x.MaHoaDon == mhd);
            foreach (var it in ods)
            {
                db.tChiTietHDBs.Remove(it);
            }
            db.SaveChanges();
            for (int i=0;i<LProduct.Count;i++)
            {
                var sz = f["LProduct[" + i + "].KichCoSP"];
                var sl = Convert.ToInt32(f["LProduct[" + i + "].SoLuongSP"]);
                var gb = Convert.ToDecimal(f["LProduct[" + i + "].GiaBanSP"]);
                var msp = f["LProduct[" + i + "].MaSP"];
                var od = db.tChiTietHDBs.FirstOrDefault(x=>x.MaHoaDon == mhd && x.MaSP == msp && x.KichCo == sz && x.GiaBan ==gb);
                if(od!=null)
                {
                    od.SoLuong += sl;
                    db.SaveChanges();
                }
                else
                {
                    od = new tChiTietHDB();
                    od.MaHoaDon = mhd;
                    od.MaSP = msp;
                    od.KichCo = sz;
                    od.GiaBan = gb;
                    od.SoLuong = sl;
                    db.tChiTietHDBs.Add(od);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}