using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyManagementSystem.Models
{
    public class CreateBill
    {
        public float SubTotal { get; set; }
        public float Discount { get; set; }
        public float GrandTotal { get; set; }

        public List<PlaceOrder> billList { get; set; }

        public CreateBill()
        {
            billList = new List<PlaceOrder>();

        }
    }
}