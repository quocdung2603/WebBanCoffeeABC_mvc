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
    
    public partial class tNhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tNhanVien()
        {
            this.tHoaDonBans = new HashSet<tHoaDonBan>();
        }
    
        public string MaNhanVien { get; set; }
        public string username { get; set; }
        public string TenNhanVien { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string ChucVu { get; set; }
        public string AnhDaiDien { get; set; }
        public string GhiChu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tHoaDonBan> tHoaDonBans { get; set; }
        public virtual tUser tUser { get; set; }
    }
}
