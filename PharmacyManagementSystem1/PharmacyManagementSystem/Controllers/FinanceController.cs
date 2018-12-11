using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PharmacyManagementSystem.Models;
namespace PharmacyManagementSystem.Controllers
{
    [Authorize]
    public class FinanceController : ApplicationBaseController
    {
        PharmacyDBEntities4 _db;
        int previousOrderQuantity;
        string previousOrderCategory;
       
        public FinanceController()
        {
            _db = new PharmacyDBEntities4();
           
        }
        // GET: Finance
        public ActionResult Index()
        {
         
            return View(_db.AllSales.ToList());
        }
        //autocomplete
        public JsonResult GetSearchValue(string search)
        {
            //List<Stock> uniqueList = new List<Stock>();
            //foreach(Stock obj in _db.Stocks)
            //{
            //    foreach(Stock ad in uniqueList)
            //    {
            //        if (ad.Name != obj.Name)
            //        {
            //            uniqueList.Add(obj);
            //        }
            //    }
            //}


            List<Stock> allsearch = _db.Stocks.Where(x => x.Name.StartsWith(search)).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetStateList(string CountryId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            List<Stock> StateList = _db.Stocks.Where(x => x.Name == CountryId).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);

        }
        // GET: Finance/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Finance/Create
        public ActionResult Create()
        {

            CreatePOS createBill = new CreatePOS();
            try
            {
                if (_db.PlaceOrders.Max(sale => sale.OrderId) == 1 && _db.AllSales.Count()==0)
                {
                    createBill.orderList = _db.PlaceOrders.ToList();
                }
                else if (_db.AllSales.Max(sale => sale.OrderId) != _db.PlaceOrders.Max(order => order.OrderId))
                {
                    createBill.orderList = _db.PlaceOrders.Where(sale => sale.OrderId > _db.AllSales.Max(order => order.OrderId)).ToList();

                }
            }
            catch
            {
            }
            return View(createBill);
        }
        
        // POST: Finance/Create
        [HttpPost]
        public ActionResult Create(PlaceOrder collection)
        {
            try
            {
                int OrderId = 1;
                int count = _db.AllSales.Count();
                if (count > 0)
                {
                    int lastOrder = _db.AllSales.Max(sale => sale.OrderId);
                    OrderId = lastOrder + 1;
                }

                  Stock obj = _db.Stocks.Where(med => med.Name == collection.Name && med.Category == collection.Category).First();
                    int subTotal = obj.SellingPrice * collection.Quantity;
                int unitPrice = obj.SellingPrice;
                    Random rand = new Random();
                    PlaceOrder order = new PlaceOrder();
                    order.SerialNumber = rand.Next();

              
                order.OrderId = OrderId;//CustomerBillId;
                    order.Name = collection.Name;
                    order.Category = collection.Category;
                    order.Quantity = collection.Quantity;
                order.UnitPrice = unitPrice;
                order.Discount = collection.Discount;
                    order.SubTotal = subTotal;
                    order.OrderDate = DateTime.Now;//collection.OrderDate;
                    _db.PlaceOrders.Add(order);

                    //update stock
                    Stock updatedStock = _db.Stocks.Where(med => med.Name == collection.Name && med.Category == collection.Category).First();
                    updatedStock.Quantity -= collection.Quantity;
                    //update stock

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
        public ActionResult ShowBill(int id)
        {
            CreateBill billDetail = new CreateBill();
            billDetail.SubTotal = float.Parse(_db.PlaceOrders.Where(obj=>obj.OrderId== id).Sum(order=>order.SubTotal).ToString());
            billDetail.Discount= float.Parse(_db.PlaceOrders.Where(obj => obj.OrderId == id).Sum(order => order.Discount).ToString());
            billDetail.GrandTotal = float.Parse(Math.Round(billDetail.SubTotal - billDetail.Discount, 3).ToString()); //billDetail.SubTotal - billDetail.Discount;
           
            billDetail.billList = _db.PlaceOrders.Where(obj => obj.OrderId == id).ToList();


            AllSale storeBill = new AllSale();
          
            storeBill.OrderId = id;
            storeBill.Date = DateTime.Now;
            storeBill.SubTotal = billDetail.SubTotal;
            storeBill.Discount = billDetail.Discount;
            storeBill.GrandTotal = billDetail.GrandTotal;
            _db.AllSales.Add(storeBill);
            _db.SaveChanges();

            return View(billDetail);
        }
        // GET: Finance/Edit/5
      // public ActionResult Edit(int id)
    /*  public ActionResult Edit(int id)
        {
            PlaceOrder Customerorder = new PlaceOrder();
            Customerorder = _db.PlaceOrders.Find(id);
            CreatePOS pos = new CreatePOS();
            pos.Name = Customerorder.Name;
            pos.Category = Customerorder.Category;
            pos.Quantity = Customerorder.Quantity;
            pos.Discount = float.Parse(Customerorder.Discount.ToString());

            return View(Customerorder);
        }*/
      
            //edit order place
        public ActionResult editsale(int id)
        {
           
            PlaceOrder Customerorder = new PlaceOrder();
            Customerorder = _db.PlaceOrders.Find(id);
            CreatePOS pos = new CreatePOS();
            pos.Name = Customerorder.Name;
         //   pos.Category = Customerorder.Category;
            ViewBag.Category = _db.Stocks.Where(x => x.Name == Customerorder.Name).Select(r => new SelectListItem { Value = r.Category, Text = r.Category }).ToList();//_db.MedicineCategories.Select(r => new SelectListItem { Value = r.Category, Text = r.Category }).ToList();
            pos.Quantity = Customerorder.Quantity;
            pos.Discount = float.Parse(Customerorder.Discount.ToString());

            //for future functionality
            previousOrderQuantity = Customerorder.Quantity;
            previousOrderCategory = Customerorder.Category;
            //for future functionality

            return View(Customerorder);
        }

        [HttpPost]
        public ActionResult editsale(int id,PlaceOrder updatedOrder)
        {
           PlaceOrder saveOrder = _db.PlaceOrders.Find(id);
            Stock updatedStock,undoStock;
            int itemPrice;
            if (saveOrder.Category == updatedOrder.Category)
            {
                updatedStock = _db.Stocks.Where(med => med.Name == saveOrder.Name && med.Category == saveOrder.Category).First();
                updatedStock.Quantity +=saveOrder.Quantity - updatedOrder.Quantity;
                //   updatedStock.Quantity -= ;
                itemPrice = updatedStock.SellingPrice;
                    TempData["msg"] = "<script>alert('same');</script>";
            }
            else
            {
                updatedStock = _db.Stocks.Where(med => med.Name == updatedOrder.Name && med.Category == updatedOrder.Category).First();
                updatedStock.Quantity -= updatedOrder.Quantity;
                itemPrice = updatedStock.SellingPrice;

                undoStock = _db.Stocks.Where(med => med.Name == saveOrder.Name && med.Category == saveOrder.Category).First();
                undoStock.Quantity += saveOrder.Quantity;



                TempData["msg"] = "<script>alert('different');</script>";
            }

            saveOrder.Name = updatedOrder.Name;
            saveOrder.Category = updatedOrder.Category;
            saveOrder.Quantity = updatedOrder.Quantity;
            saveOrder.Discount = updatedOrder.Discount;
            saveOrder.SubTotal = itemPrice * updatedOrder.Quantity;
            saveOrder.OrderDate = DateTime.Now;
            _db.SaveChanges();


            //check either it is being edited after bill print or not and redirect to specific direction
            if (_db.AllSales.Where(sale => sale.OrderId == saveOrder.OrderId).Count() != 0)
            {
                return RedirectToAction("EditBill"+"/"+saveOrder.OrderId.ToString());
            }
            else
            {
                return RedirectToAction("Create");
            }
         //   return View(Customerorder);
        }

        public ActionResult Delete(int id)
        {
            PlaceOrder selected = _db.PlaceOrders.Find(id);
            

            //update stock
            Stock updateStock = _db.Stocks.Where(med => med.Name == selected.Name && med.Category == selected.Category).First();
            updateStock.Quantity += selected.Quantity;
            //update stock

            _db.PlaceOrders.Remove(selected);
            _db.SaveChanges();
            return RedirectToAction("Create");
            //   return View();
        }

        //delete complete order
        public ActionResult DeleteOrder(int id)
        {
            List<PlaceOrder> sales = _db.PlaceOrders.Where(sale => sale.OrderId == id).ToList();
            _db.PlaceOrders.RemoveRange(sales);
             AllSale deleteOrder = _db.AllSales.Find(id);
            _db.AllSales.Remove(deleteOrder);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        //edit order after print bill
        public ActionResult EditBill(int id)
        {

            ViewBag.OrderId = id;
            List<PlaceOrder> selectSales = _db.PlaceOrders.Where(order => order.OrderId == id).ToList();
          //  List<PlaceOrder> filtered = selectSales.Where(order => order.OrderId == id).ToList();
            return View(selectSales);
        }

        [HttpPost]
        public ActionResult EditBill(int id,AllSale allsale)
        {
            float SubTotal= float.Parse(_db.PlaceOrders.Where(obj => obj.OrderId == id).Sum(order => order.SubTotal).ToString());
            float Discount = float.Parse(_db.PlaceOrders.Where(obj => obj.OrderId == id).Sum(order => order.Discount).ToString());
            float GrandTotal = SubTotal - Discount;
            AllSale updateBill = _db.AllSales.Find(id);
            updateBill.Date = DateTime.Now;
            updateBill.SubTotal = SubTotal;
            updateBill.Discount = Discount;
            updateBill.GrandTotal = GrandTotal;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult CheckOrderExists(string Name,string Category)
        {
            bool UserExists = false;
            try
            {
                var nameexits = _db.PlaceOrders.Where(x =>x.OrderId > _db.AllSales.Max(sale=>sale.OrderId) && x.Name == Name && x.Category==Category).ToList();
                if (nameexits.Count() > 0)
                {
                    UserExists = true;
                }
                else
                {
                    UserExists = false;
                }
                return Json(!UserExists, JsonRequestBehavior.AllowGet);
            }

            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        
             public JsonResult CheckQuantityExists(string Name, string Category,int Quantity)
        {
            bool UserExists = false;
            try
            {
                var validQuantity = _db.Stocks.Where(x => x.Name == Name && x.Category == Category).First();

                if (validQuantity.Quantity < Quantity)
                {
                    UserExists = true;
                }
                else
                {
                    UserExists = false;
                }
                return Json(!UserExists, JsonRequestBehavior.AllowGet);
            }

            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



        // GET: Finance/ViewExpenses
        public ActionResult ViewExpenses()
        {
            return View(_db.Expenses.ToList());
        }

  


        // GET: Finance/AddExpenses
        public ActionResult AddExpenses()
        {
            Expens expenses = new Expens();
          //  expenses.CategoryList = null;
            expenses.CategoryList = new SelectList(_db.ExpenseCategories.ToList(), "Category", "Category");

            return View(expenses);
        }

        // POST: Finance/AddExpenses
        [HttpPost]
        public ActionResult AddExpenses(Expens collection)
        {
            try
            {
                Random rand = new Random();
                Expens NewExpense = new Expens();
                NewExpense.SerialNumber = rand.Next(10000);
                NewExpense.Amount = collection.Amount;
                NewExpense.Category = collection.Category;
                NewExpense.Date = collection.Date;//DateTime.Now;
                _db.Expenses.Add(NewExpense);
                _db.SaveChanges();
                // TODO: Add insert logic here

                // return RedirectToAction("PrintBill");
                return RedirectToAction("ViewExpenses");
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
        public ActionResult AddExpenseCategory(ExpenseCategory collection)
        {
            try
            {
                ExpenseCategory NewCategory = new ExpenseCategory();
                NewCategory.Category = collection.Category;
                NewCategory.Description = collection.Description;
                _db.ExpenseCategories.Add(NewCategory);
                _db.SaveChanges();
                return RedirectToAction("AddExpenses");
            }
            catch
            {
                return View();
            }
        }
        // POST: Finance/Edit/5
      /*  [HttpPost]
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
        */
        // GET: Finance/Delete/5
      /*  public ActionResult Delete(int id)
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

    */

        public ActionResult SaleToday()
        {
            var currentdate = DateTime.Today.ToString("yyyy-MM-dd");

            return View(_db.AllSales.Where(sale => sale.Date.ToString() == currentdate).ToList());
        }
        public ActionResult ExpenseToday()
        {
            return View(_db.Expenses.ToList());
        }

        public ActionResult EditExpense(int id)
        {
            Expens editExpense = _db.Expenses.Find(id);
          //  editExpense.Date = editExpense.Date.Date;//DateTime.Parse(editExpense.Date.ToString("yyyy/MM/dd"));//editExpense.Date.ToString("yyyy/MM/dd");//ToString("yyyy-MM-dd");
            return View(editExpense);
        }
        [HttpPost]
        public ActionResult EditExpense(int id,Expens updatedExpense)
        {
            Expens expense = _db.Expenses.Find(id);
            expense.Amount = updatedExpense.Amount;
            expense.Category = updatedExpense.Category;
            expense.Date = updatedExpense.Date;
            _db.SaveChanges();
            return RedirectToAction("ViewExpenses");
        }

        public ActionResult DeleteExpense(int id)
        {
            Expens editExpense = _db.Expenses.Find(id);
            _db.Expenses.Remove(editExpense);
            _db.SaveChanges();
            return RedirectToAction("ViewExpenses");
        }

    }
}
