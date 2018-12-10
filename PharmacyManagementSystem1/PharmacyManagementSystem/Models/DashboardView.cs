using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyManagementSystem.Models
{
    public class DashboardView
    {
        public int SalesToday { get; set; }
        public int ExpeneseToday { get; set; }
        public int CountStock { get; set; }
        public int CountStaff { get; set; }
        public int CountSales { get; set; }
        public int TotalSales { get; set; }

        public int CountExpenses { get; set; }
        public int TotalExpenses { get; set; }
        public int StockInventoryQuantity { get; set; }

        public int CountOutStockMedicine { get; set; }
   
        public List<AllSale> LatestSale { get; set; }
        public DashboardView()
        {
            LatestSale = new List<AllSale>();
        }
    }
}