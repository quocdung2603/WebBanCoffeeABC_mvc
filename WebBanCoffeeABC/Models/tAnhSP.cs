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
    
    public partial class tAnhSP
    {
        public string MaSP { get; set; }
        public string TenFileAnh { get; set; }
        public bool AnhMacDinh { get; set; }
        public int Id { get; set; }
    
        public virtual tDanhMucSP tDanhMucSP { get; set; }
    }
}
