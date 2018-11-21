//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PharmacyManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public partial class Stock
    {
        public string SerialNumber { get; set; }
        [Remote("CheckUserNameExists", "Stock", ErrorMessage = "Name already exists!")]
        public string Name { get; set; }
        public string Category { get; set; }
        public int PurchasePrice { get; set; }
        public int SellingPrice { get; set; }
        
        public int Quantity { get; set; }

        public System.DateTime ExpiryDate { get; set; }
        public System.DateTime AddedDate { get; set; }
    }
}