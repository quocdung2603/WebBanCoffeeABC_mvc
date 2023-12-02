using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanCoffeeABC.Models
{
    public class Cart
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        public string Productid { get; set; }
        public string ProductName { get; set; }
        //public string ProductImage { get; set; }
        public string Manufacturer { get; set; }
        public string Productintroduction { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductNumber { get; set; }
        public int Sizeid { get; set; }
        //public int Colorid { get; set; }
        public decimal TotalProductPrice
        {
            get { return ProductNumber * ProductPrice; }
        }
        public Cart(string Productid, int sizeid, int colorid, int numproduct)
        {
            this.Productid = Productid;
            tDanhMucSP s = db.tDanhMucSPs.SingleOrDefault(n => n.MaSP == this.Productid);
            this.ProductName = s.TenSP;
            this.Manufacturer = s.MaHangSX;
            this.Productintroduction = s.GioiThieuSP;
            //this.ProductImage = s.Illsutration;
            this.ProductPrice = decimal.Parse(s.GiaGoc.ToString());
            this.ProductNumber = 1;
            //this.Colorid = colorid;
            this.Sizeid = sizeid;
            this.ProductNumber = numproduct;
        }
    }
}