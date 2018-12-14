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
                return RedirectToAction("MedicineView");

            }
            catch
            {
                return View();
            }
        }
        public ActionResult CheckStockExists(string Name, string Category)
        {
            bool UserExists = false;
            try
            {
                var stockexist = _db.Stocks.Where(x => x.Name == Name & x.Category == Category).ToList();

                if (stockexist.Count() > 0)
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

            try
            {
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

        public JsonResult CheckSellingPrice(int PurchasePrice, int SellingPrice)

        {
            bool UserExists = false;

            try
            {

                if (SellingPrice <= PurchasePrice)

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

        public JsonResult CheckExpiryDate(DateTime ExpiryDate)

        {
            bool UserExists = false;

            try
            {
                if (DateTime.Parse(ExpiryDate.ToString()) <= DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd")))

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
            return PartialView("_addStock", Stock);

        }
        [HttpPost]
        public ActionResult addStock(Stock collection)
        {
            try
            {
                if (_db.Stocks.Where(medicine => medicine.Name == collection.Name && medicine.Category == collection.Category).ToList().Count > 0)
                {
                    TempData["msg"] = "<script>alert('Stock Already Exist');</script>";

                    return RedirectToAction("AddMedicine");
                }

                else if (collection.SellingPrice <= collection.PurchasePrice)
                {
                    TempData["msg"] = "<script>alert('Selling Price must be greater than purchase price');</script>";

                    return RedirectToAction("AddMedicine");
                }


                else if (DateTime.Parse(collection.ExpiryDate.ToString()) <= DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd")))
                {
                    TempData["msg"] = "<script>alert('Expiry date must be greater than current date');</script>";

                    return RedirectToAction("AddMedicine");
                }
                else
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

                    TempData["msg"] = "<script>alert('Stock Added successfully');</script>";

                    return RedirectToAction("MedicineView");
                }

            }
            catch
            {
                TempData["msg"] = "<script>alert('Invalid Stock Entry');</script>";
                return RedirectToAction("AddMedicine");
            }


        }

        [Authorize(Roles = "Admin")]
        // GET: Stock/Edit/5
        public ActionResult EditStock(string id)
        {

            //  Stock selectedMedicine = new Stock();
            Stock selectedMedicine = _db.Stocks.Find(id);
            selectedMedicine.CategoryList = new SelectList(_db.MedicineCategories.ToList(), "Category", "Category");


            return View(selectedMedicine);
        }

        // POST: Stock/Edit/5
        [HttpPost]
        public ActionResult EditStock(string id, Stock collection)
        {
            try
            {
                if (collection.SellingPrice <= collection.PurchasePrice)
                {

                    TempData["msg"] = "<script>alert('Stock can not updated');</script>";
                    collection.CategoryList = new SelectList(_db.MedicineCategories.ToList(), "Category", "Category");
                    return View(collection);
                }
                else
                {
                    Stock s = _db.Stocks.Find(id);
                    s.Name = collection.Name;
                    s.Category = collection.Category;
                    s.PurchasePrice = collection.PurchasePrice;
                    s.SellingPrice = collection.SellingPrice;
                    s.Quantity = collection.Quantity;
                    s.ExpiryDate = collection.ExpiryDate;
                    _db.SaveChanges();

                    TempData["msg"] = "<script>alert('Stock updated successfully');</script>";
                    return RedirectToAction("MedicineView");
                }
            }
            catch
            {

                TempData["msg"] = "<script>alert('Stock can not updated');</script>";
                collection.CategoryList = new SelectList(_db.MedicineCategories.ToList(), "Category", "Category");

                return View(collection);
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Staff/Edit/5

        public ActionResult Edit(string id)
        {
            Stock selectedMedicine = _db.Stocks.Find(id);//_db.Staffs.Where(x => x.Email == id).First();
            selectedMedicine.CategoryList = new SelectList(_db.MedicineCategories.ToList(), "Category", "Category");

            return PartialView("_editStock", selectedMedicine);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Stock collection)
        {
            try
            {
                if (collection.SellingPrice <= collection.PurchasePrice)
                {
                    TempData["msg"] = "<script>alert('Stock can not updated');</script>";
                    return RedirectToAction("EditStock/" + id);
                }
                else
                {
                    Stock s = _db.Stocks.Find(id);
                    s.Name = collection.Name;
                    s.Category = collection.Category;
                    s.PurchasePrice = collection.PurchasePrice;
                    s.SellingPrice = collection.SellingPrice;
                    s.Quantity = collection.Quantity;
                    s.ExpiryDate = collection.ExpiryDate;
                    _db.SaveChanges();

                    TempData["msg"] = "<script>alert('Stock updated successfully');</script>";
                    return RedirectToAction("MedicineView");
                }
            }
            catch
            {
                TempData["msg"] = "<script>alert('Stock can not updated');</script>";
                return RedirectToAction("EditStock/" + id);
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
                s.Quantity = s.Quantity + collection.Quantity;
                _db.SaveChanges();
                TempData["msg"] = "<script>alert('Stock Quantity updated successfully');</script>";
                return RedirectToAction("MedicineView");

            }
            catch
            {
                TempData["msg"] = "<script>alert('Stock Quantity can not updated');</script>";
                return RedirectToAction("EditQuantity/" + id);
            }
        }


        [Authorize(Roles = "Admin")]
        public ActionResult EditQuantity(string id)
        {
            Stock selectedMedicine = _db.Stocks.Find(id);
            return View(selectedMedicine);
        }

        // POST: Stock/EditQuantity/5
        [HttpPost]
        public ActionResult EditQuantity(string id, Stock collection)
        {
            try
            {
                Stock s = _db.Stocks.Find(id);
                s.Quantity = s.Quantity + collection.Quantity;
                _db.SaveChanges();
                TempData["msg"] = "<script>alert('Stock Quantity updated successfully');</script>";
                return RedirectToAction("MedicineView");

            }
            catch
            {
                TempData["msg"] = "<script>alert('Stock Quantity can not updated');</script>";
                return View();
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

        }
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
                MedicineCategory category = new MedicineCategory();
                category.Category = collection.Category;
                _db.MedicineCategories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("MedicineView");
            }
            catch
            {
                return View();
            }
        }

        //Get:Stock/ShowCategory
        public ActionResult MedicineCategory()
        {
            return View(_db.MedicineCategories.ToList());
        }
    }
}
