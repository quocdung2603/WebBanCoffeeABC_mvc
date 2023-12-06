
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Drawing;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using CozaStore.Models;
//using CozaStore.Others;
//using WebBanCoffeeABC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanCoffeeABC.Models;
using WebBanCoffeeABC.Others;

namespace WebBanCoffeeABC.Controllers
{
    public class CartController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        public List<Cart> GetCart()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }

        public ActionResult AddCart(string productid, string url, FormCollection f)
        {
            List<Cart> lstCart = GetCart();
            Cart product = lstCart.Find(n => n.Productid == productid);
            string sizeid = f["Type-Size"].ToString();
            
            int numproduct = int.Parse(f["num-product"]);
            if (product == null)
            {
                product = new Cart(productid, sizeid, numproduct);
                lstCart.Add(product);
            }
            else
            {
                product.ProductNumber += numproduct;
            }
            return Redirect(url);
        }

        private int TotalNumberProduct()
        {
            int totalNumberProduct = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                totalNumberProduct = lstCart.Sum(n => n.ProductNumber);
            }
            return totalNumberProduct;
        }
        // GET: Cart
        private decimal TotalPrice()
        {
            decimal TotalPrice = decimal.Zero;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                TotalPrice = lstCart.Sum(n => n.TotalProductPrice);
            }
            return TotalPrice;
        }
        
        List<tKichThuoc> FullSize()
        {
            return db.tKichThuocs.ToList();
        }

        public ActionResult Cart()
        {
            List<Cart> lstCart = GetCart();
            if (lstCart.Count == 0)
            {
                return RedirectToAction("Index", "ABCHome");
            }
            ViewBag.Number = TotalNumberProduct();
            ViewBag.TotalPrice = TotalPrice();
            ViewBag.Size = FullSize();
            ViewBag.lstCart  = lstCart; 
            return View(lstCart);
        }

        public ActionResult DeleteProduct(string productid)
        {
            List<Cart> lstCart = GetCart();
            Cart product = lstCart.SingleOrDefault(n => n.Productid == productid);
            if (product != null)
            {
                lstCart.RemoveAll(n => n.Productid == productid);
                if (lstCart.Count == 0)
                {
                    return RedirectToAction("Index", "ABCHome");
                }
            }
            return RedirectToAction("Cart");
        }

        public ActionResult UpdateCart(string productid, FormCollection f)
        {
            List<Cart> lstcart = GetCart();
            if (lstcart == null)
            {
                return RedirectToAction("Cart", "Cart");
            }
            Cart product = lstcart.SingleOrDefault(n => n.Productid == productid);
            if (product == null)
            {
                return RedirectToAction("Cart", "Cart");
            }
            int num = int.Parse(f["num-product"]);
            product.ProductNumber = num;
            return RedirectToAction("Cart", "Cart");
        }
        public ActionResult ClearCart()
        {
            List<Cart> lstCart = GetCart();
            lstCart.Clear();
            return RedirectToAction("Index", "ABCHome");

        }
        [ChildActionOnly]
        public ActionResult ViewCard()
        {
            ViewBag.Number = TotalNumberProduct();
            return PartialView();
        }
        public ActionResult ShoppingCard()
        {
            
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            List<Cart> lstcart = GetCart();
            ViewBag.Number = TotalNumberProduct();
            ViewBag.TotalPrice = TotalPrice();
            ViewBag.Size = FullSize();
            ViewBag.lstCart = lstcart;
            return View(lstcart);
        }

        
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                //string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();
                //create bill
                tHoaDonBan bill = new tHoaDonBan();
                tUser user = Session["User"] as tUser;
                var kh = db.tKhachHangs.FirstOrDefault(h => h.username == user.username);
                bill.MaKhachHang = kh.MaKhachHang;
               // bill.TotalPrice = TotalPrice();
                bill.GhiChu = " ";
               
                bill.NgayHoaDon = DateTime.Now;
                //end create bill
                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về
                bill.MaHoaDon = orderId.ToString();
               // bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                //if (checkSignature)
                //{
                //    if (vnp_ResponseCode == "00")
                //    {
                //        //Thanh toán thành công
                //        ViewBag.Message = "Payment success";
                //        ViewBag.Bill = bill;
                //        db.tHoaDonBans.Add(bill);
                //        db.SaveChanges();
                //        List<Cart> lstcart = GetCart();
                //        foreach (var item in lstcart)
                //        {
                //            tChiTietHDB value = new tChiTietHDB()
                //            {
                //                MaSP = item.Productid,
                //                MaHoaDon = bill.MaHoaDon,
                //                KichCo = item.Sizeid,
                               
                //                SoLuong = item.ProductNumber,
                //               // TotalPrice = item.TotalProductPrice
                //            };
                //            db.tChiTietHDBs.Add(value);
                //        }
                //        db.SaveChanges();
                //        ClearCart();
                //    }
                //    else
                //    {
                //        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                //        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                //    }
                //}
                //else
                //{
                //    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                //}
            }

            return View();
        }
        string getbillid()
        {
            Random rd = new Random();
            string tmp = "HD" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            return tmp;
        }
        public ActionResult delivery()
        {
            string billid = getbillid();
            tHoaDonBan bill = new tHoaDonBan();
            tUser user = Session["User"] as tUser;
            var kh = db.tKhachHangs.FirstOrDefault(h => h.username == user.username);
            bill.MaKhachHang = kh.MaKhachHang;
            bill.TongTienHD = TotalPrice();
            
            bill.GhiChu = "";
            bill.NgayHoaDon = DateTime.Now;
            bill.MaHoaDon = billid;
            ViewBag.Message = "Payment success";
            ViewBag.Bill = bill;
            db.tHoaDonBans.Add(bill);
            db.SaveChanges();
            List<Cart> lstcart = GetCart();
            foreach (var item in lstcart)
            {
                tChiTietHDB value = new tChiTietHDB()
                {
                    MaSP = item.Productid,
                    MaHoaDon = bill.MaHoaDon,
                    KichCo = item.Sizeid,
                    SoLuong = item.ProductNumber,
                    
                    
                };
                db.tChiTietHDBs.Add(value);
            }
            db.SaveChanges();
            ClearCart();
            return View();
        }
    }
}