using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyManagementSystem.Models
{
    public class CreatePOS
    {
        [Required]

        public string Name { get; set; }

        [Required]
        [Remote("CheckOrderExists", "Finance", ErrorMessage = "Order Already has been added", AdditionalFields = "Name")]

        public string Category { get; set; }
        [Required]
        [Range(typeof(int), "0", "10000", ErrorMessage = "Quantity must be greater than 0")]
      [Remote("CheckQuantityExists", "Finance", ErrorMessage = "Required quantity is not present", AdditionalFields = "Name,Category")]
     

        public int Quantity { get; set; }
        [Required]
        [Range(typeof(float), "0", "10000", ErrorMessage = "Discount must be greater than 0")]
        public float Discount { get; set; }

      
        public List<PlaceOrder> orderList { get; set; }
        
        public CreatePOS()
        {
            orderList = new List<PlaceOrder>();
          
        }
    }
}