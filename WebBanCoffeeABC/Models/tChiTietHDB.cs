//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebBanCoffeeABC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tChiTietHDB
    {
        public string MaHoaDon { get; set; }
        public string MaSP { get; set; }
        public string KichCo { get; set; }
        public Nullable<decimal> GiaBan { get; set; }
        public Nullable<int> SoLuong { get; set; }
    
        public virtual tDanhMucSP tDanhMucSP { get; set; }
        public virtual tHoaDonBan tHoaDonBan { get; set; }
    }
}
