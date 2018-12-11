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
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Stock
    {
        public string SerialNumber { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Remote("CheckStockExists", "Stock", ErrorMessage = "Stock Already Exist", AdditionalFields = "Name")]
        public string Category { get; set; }
        public System.Web.Mvc.SelectList CategoryList { get; set; }
        [Required]
        [Range(typeof(int), "1", "10000", ErrorMessage = "Purchase Price must be greater than 0")]
        public int PurchasePrice { get; set; }
        [Required]
        [Range(typeof(int), "1", "10000", ErrorMessage = "Selling Price must be greater than 0")]
        [Remote("CheckSellingPrice", "Stock", ErrorMessage = "Selling Price must be greater than purchase price", AdditionalFields = "PurchasePrice")]

        public int SellingPrice { get; set; }
        [Required]
        [Range(typeof(int), "1", "10000", ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Remote("CheckExpiryDate", "Stock", ErrorMessage = "Expiry date must be greater than current date")]

        public System.DateTime ExpiryDate { get; set; }

        public System.DateTime AddedDate { get; set; }
    }
}
