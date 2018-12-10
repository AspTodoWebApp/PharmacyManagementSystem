using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PharmacyManagementSystem.Models;
namespace PharmacyManagementSystem.Controllers
{[Authorize]
    public class DashboardController : ApplicationBaseController // Controller
    {
        PharmacyDBEntities4 _db;
        public DashboardController()

        {
            _db = new PharmacyDBEntities4();
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            var currentdate = DateTime.Today.ToString("yyyy-MM-dd");
            DashboardView dashboard = new DashboardView();
           try { dashboard.SalesToday = int.Parse(_db.AllSales.Where(exp => exp.Date.ToString() == currentdate).Sum(sm => sm.SubTotal).ToString()); }
            catch { dashboard.SalesToday = 0; }

            try { dashboard.ExpeneseToday = int.Parse(_db.Expenses.Where(exp => exp.Date.ToString() == currentdate).Sum(sm => sm.Amount).ToString()); }
            catch { dashboard.ExpeneseToday = 0; }

            dashboard.CountStock = _db.Stocks.Count();
            dashboard.CountStaff = _db.Staffs.Count();
            dashboard.CountSales = _db.AllSales.Count();

            try { dashboard.TotalSales = int.Parse(_db.AllSales.Sum(sm => sm.SubTotal).ToString()); }
            catch { dashboard.TotalSales = 0; }

            dashboard.CountExpenses = _db.Expenses.Count();

            try { dashboard.TotalExpenses = int.Parse(_db.Expenses.Sum(sm => sm.Amount).ToString()); }
            catch { dashboard.TotalExpenses = 0; }

            try { dashboard.StockInventoryQuantity = int.Parse(_db.Stocks.Sum(med => med.Quantity * med.PurchasePrice).ToString()); }
            catch { dashboard.StockInventoryQuantity = 0; }

            dashboard.CountOutStockMedicine = _db.Stocks.Where(med => med.Quantity <= 50).Count();


            dashboard.LatestSale = _db.AllSales.Where(sale => sale.Date.ToString() == currentdate).ToList();

            return View(dashboard);
        }

        // GET: Dashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Dashboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dashboard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Dashboard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dashboard/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    
        
    }
}
