using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PharmacyManagementSystem.Models;
namespace PharmacyManagementSystem.Controllers
{
    [Authorize]
    public class StockController : ApplicationBaseController //Controller
    {
        PharmacyDBEntities4 _db;
        
        public StockController()
        {
            _db = new PharmacyDBEntities4();
        }

        // GET: Stock
     //  [Authorize(Roles = "Admin")]
        public ActionResult MedicineView()
        {
            return View(_db.Stocks.ToList());
        }
     

        // GET: Stock/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        // GET: Stock/Create
        public ActionResult AddMedicine()
        {
            var Stock = new Stock();
            Stock.CategoryList = new SelectList(_db.MedicineCategories.ToList(), "Category", "Category");
            return View(Stock);
        }

        // POST: Stock/Create
        [HttpPost]
        public ActionResult AddMedicine(Stock collection)
        {
            try
            {
               Stock med = new Stock();
                    med.SerialNumber = Guid.NewGuid().ToString();
                    med.Name = collection.Name;
                    med.PurchasePrice = collection.PurchasePrice;
                    med.SellingPrice = collection.SellingPrice;
                    med.ExpiryDate = collection.ExpiryDate;
                    med.Quantity = collection.Quantity;
                    med.Category = collection.Category;
                    med.AddedDate = DateTime.Now;
             
                    _db.Stocks.Add(med);
                    _db.SaveChanges();
                //string a = "hello";
                
                // TODO: Add insert logic here
            
              
               return RedirectToAction("MedicineView");
             // }   
            }
            catch
            {
               return View();
            }
        }



        
             public JsonResult CheckStockExists(string Name,string Category)

        {
            bool UserExists = false;

            try
            {


                var nameexits = _db.Stocks.Where(x => x.Name == Name & x.Category == Category).ToList();

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
       public JsonResult CheckCategoryExists(string Category)

        {
 bool UserExists = false;

            try { 

            
                var nameexits = _db.Stocks.Where(x => x.Category == Category).ToList();

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


        [Authorize(Roles = "Admin")]
        public ActionResult addStock()
        {
            var Stock = new Stock();
            Stock.CategoryList = new SelectList(_db.MedicineCategories.ToList(), "Category", "Category");
            return PartialView("_addStock",Stock);

        }
        [HttpPost]
        public ActionResult addStock(Stock collection)
        {
            try
            {
                Stock med = new Stock();
                med.SerialNumber = Guid.NewGuid().ToString();
                med.Name = collection.Name;
                med.PurchasePrice = collection.PurchasePrice;
                med.SellingPrice = collection.SellingPrice;
                med.ExpiryDate = collection.ExpiryDate;
                med.Quantity = collection.Quantity;
                med.Category = collection.Category;
                med.AddedDate = DateTime.Now;
                _db.Stocks.Add(med);
                _db.SaveChanges();

                return RedirectToAction("MedicineView");

            }
            catch
            {
                return RedirectToAction("AddMedicine");
            }


        }


        // GET: Stock/Edit/5
        /* public ActionResult Edit(int id)
         {
             return View();
         }

         // POST: Stock/Edit/5
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
         }*/

        [Authorize(Roles = "Admin")]
        // GET: Staff/Edit/5
        //   public ActionResult Edit(int id)
        public ActionResult Edit(string id)
        {
            Stock selectedMedicine = _db.Stocks.Find(id);//_db.Staffs.Where(x => x.Email == id).First();

            return PartialView("_editStock", selectedMedicine);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Stock collection)
        {
            try
            {
           
                Stock s = _db.Stocks.Find(id);
                s.Name = collection.Name;
                s.Category = collection.Category;
                 s.PurchasePrice = collection.PurchasePrice;
                s.SellingPrice = collection.SellingPrice;
                s.Quantity = collection.Quantity;
                s.ExpiryDate = collection.ExpiryDate;
                _db.SaveChanges();
                return RedirectToAction("MedicineView");
            }
            catch
            {
                return PartialView("_editStock");
            }
        }

        [Authorize(Roles = "Admin")]
        //POST: Stock/EditQuantity/5
        public ActionResult LoadQuantity(string id)
        {
            Stock selectedMedicine = _db.Stocks.Find(id);

            return PartialView("_loadQuantity", selectedMedicine);
        }

        // POST: Stock/EditQuantity/5
        [HttpPost]
        public ActionResult LoadQuantity(string id, Stock collection)
        {
            try
            {
               Stock s = _db.Stocks.Find(id);
                s.Quantity = collection.Quantity;
                _db.SaveChanges();
                return RedirectToAction("MedicineView");
            }
            catch
            {
                return PartialView("_loadQuantity");
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Stock/Delete/5
        public ActionResult Delete(string id)
        {
            Stock selected = _db.Stocks.Find(id);
            _db.Stocks.Remove(selected);
            _db.SaveChanges();
            return RedirectToAction("MedicineView");
            //   return View();
        }
        
        // POST: Stock/Delete/5
     /*   [HttpPost]
        public ActionResult Delete(int id, Stock collection)
        {
            try
            {

                // TODO: Add delete logic here
                Stock selected = _db.Stocks.Find(id);
                _db.Stocks.Remove(selected);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
                // return View();
            }
        }
        */



        // GET: Stock/StockAlert/5
        public ActionResult StockAlert()
        {
            List<Stock> stockAlertMedicines = _db.Stocks.Where(medicine => medicine.Quantity <= 50).ToList();
            return View(stockAlertMedicines);
        }

   


        // GET: Stock/StockAlert/5
        public ActionResult ExpiryAlert()
        {
            List<Stock> expiryAlertMedicines = _db.Stocks.Where(medicine => medicine.ExpiryDate <= DateTime.Now).ToList();
            return View(expiryAlertMedicines);
        }


        [Authorize(Roles = "Admin")]

        // GET: Stock/Category/5
        public ActionResult AddNewCategory()
        {
           
            return View();
        }

        // POST: Stock/Category/5
        [HttpPost]
        public ActionResult AddNewCategory(MedicineCategory collection)
        {
            try
            {
                // TODO: Add delete logic here
                MedicineCategory category = new MedicineCategory();
                category.Category = collection.Category;
                _db.MedicineCategories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("MedicineView");
                //  return RedirectToAction("MedicineCategory");
            }
            catch
            {
                return View();
            }
        }

        //Get:Stock/ShowCategory
        public ActionResult MedicineCategory()
        {
            
                // TODO: Add delete logic here

                return View(_db.MedicineCategories.ToList());
           
        }


    }
}
