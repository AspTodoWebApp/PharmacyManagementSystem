using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PharmacyManagementSystem.Models;
namespace PharmacyManagementSystem.Controllers
{
    public class FinanceController : ApplicationBaseController
    {
        PharmacyDBEntities2 _db;
        public FinanceController()
        {
            _db = new PharmacyDBEntities2();
        }
        // GET: Finance
        public ActionResult Index()
        {
            
            return View(_db.AllSales.ToList());
        }

        // GET: Finance/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Finance/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Finance/Create
        [HttpPost]
        public ActionResult Create(PlaceOrder collection)
        {
            try
            {
                // TODO: Add insert logic here

                // return RedirectToAction("PrintBill");
                PlaceOrder order = new PlaceOrder();
                order.SerialNumber = _db.PlaceOrders.ToList().Count() + 1;
                order.OrderId = 7878;// collection.OrderId;
                order.Name = collection.Name;
                order.Category = collection.Category;
                order.SubTotal = 878;//collection.SubTotal;
                order.OrderDate = DateTime.Now;//collection.OrderDate;
                _db.PlaceOrders.Add(order);
                _db.SaveChanges();
                TempData["msg"] = "<script>alert('item added');</script>";
                return RedirectToAction("Create");

               // return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Finance/ShowBill
        public ActionResult ShowBill()
        {
            return View();
        }
        // GET: Finance/Edit/5
      // public ActionResult Edit(int id)
      public ActionResult Edit()
        {
            return View(_db.PlaceOrders.ToList());
        }

        // GET: Finance/ViewExpenses
        public ActionResult ViewExpenses()
        {
            return View(_db.Expenses.ToList());
        }

  


        // GET: Finance/AddExpenses
        public ActionResult AddExpenses()
        {
            return View();
        }

        // POST: Finance/AddExpenses
        [HttpPost]
        public ActionResult AddExpenses(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                // return RedirectToAction("PrintBill");
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Finance/AddExpenseCategory
        public ActionResult AddExpenseCategory()
        {
            return View();
        }

        // POST: Finance/AddExpenseCategory
        [HttpPost]
        public ActionResult AddExpenseCategory(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                // return RedirectToAction("PrintBill");
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // POST: Finance/Edit/5
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

        // GET: Finance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Finance/Delete/5
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



        public ActionResult SaleToday()
        {
            return View(_db.AllSales.ToList());
        }
        public ActionResult ExpenseToday()
        {
            return View(_db.Expenses.ToList());
        }

        public ActionResult EditExpense()
        {
            return View();
        }
      
    }
}
