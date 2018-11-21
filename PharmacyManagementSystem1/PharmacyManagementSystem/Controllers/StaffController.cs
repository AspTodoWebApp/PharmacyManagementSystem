using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PharmacyManagementSystem.Models;
using System.Text;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using Microsoft.Owin.Security;
using System.Security.Cryptography;
using System.Web.Security;

namespace PharmacyManagementSystem.Controllers
{
    public class StaffController : ApplicationBaseController
    {
        PharmacyDBEntities2 _db;
       
   
      public  StaffController()
        {
            _db = new PharmacyDBEntities2();
        }
        // GET: Staff
    
 // [Authorize(Roles ="Manager")]
  
        public ActionResult Index()
        {
         //   Roles.AddUserToRole(User.Identity.GetUserName(), "Admin");
           
            // AuthorizeAttribute roles = new AuthorizeAttribute();

            ViewBag.type = 0;
            //    string loginId = User.Identity.GetUserId();
            //     string role = _db.AspNetRoles.Find(loginId).Name;
            //if (LoginRole.role == "Staff")
            //{
            //    return RedirectToAction("addStaff");
            //}
            //else
            //{

                return View(_db.Staffs.ToList());
           
               
          // }
        }

        public ActionResult myModal(string Id)
        {
            ViewBag.type = 1;
            Staff employee = _db.Staffs.Where(st=>st.Name==Id).First();
            if (employee == null)
            {
                return HttpNotFound();
            }
            return PartialView("_myModal", employee);

        }
  //  [Authorize(Roles ="Admin")]
  
        public ActionResult addStaff()
        {
            
            return PartialView("_addStaff");
         
        } 
        [HttpPost]
        public ActionResult addStaff(Staff s)
        {
            try {

                Staff st = new Staff();
                st.Name = s.Name;
                st.Username = s.Username;
                st.Email = s.Email;
                st.Phone = s.Phone;
                st.Address = s.Address;
                _db.Staffs.Add(st);
                //String builder = new StringB();
                //builder.Append(GetRandomString(2));
                //builder.Append(RandomNumber(1000, 9999));
                //builder.Append(RandomString(2, false));


                string random = GetRandomString(5);
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                   client.EnableSsl = true;
                   //  client.Timeout = 1000000;
                   client.DeliveryMethod = SmtpDeliveryMethod.Network;
                   client.UseDefaultCredentials = false;
                   client.Credentials = new NetworkCredential("numanuet311@gmail.com", "47841271");
                   MailMessage msg = new MailMessage();
                   msg.To.Add("afshanarsha2783@gmail.com");
                   msg.From = new MailAddress("numanuet311@gmail.com");
                   msg.Subject = "Staff added by admin";
                   msg.Body = random;
                   client.Send(msg);
             //     random = "Numan311@";
                var hasher = new PasswordHasher();
                  AspNetUser staff = new AspNetUser();
                staff.Id = Guid.NewGuid().ToString();
                  staff.Email = s.Email;
                staff.EmailConfirmed = false;
                staff.TwoFactorEnabled = false;
                staff.LockoutEnabled = true;
                staff.AccessFailedCount = 0;
                  staff.SecurityStamp =Guid.NewGuid().ToString();
                staff.PasswordHash = hasher.HashPassword(random);
                    staff.UserName = s.Email;
                _db.AspNetUsers.Add(staff);

                AspNetRole role = new AspNetRole();
                role.Id = staff.Id;
                role.Name = "Staff";
                _db.AspNetRoles.Add(role);
                    _db.SaveChanges();
               
               
                 TempData["msg"] = "<script>alert('added');</script>";

                return RedirectToAction("Index");
                
            }
            catch
            {
                return PartialView("_addStaff");
            }
           

        }


        public JsonResult GetSearchingData(string SearchBy,string SearchValue)
          {
              List<Staff> stafflist = new List<Staff>();
              if (SearchBy == "Name")
              {
                  try
                  {
                        stafflist = _db.Staffs.Where(x => x.Name.StartsWith(SearchValue)).ToList();


                  }
                  catch(FormatException)
                  {
                      Console.WriteLine("{0} is Not A Name ", SearchValue);
                  }
                  return Json(stafflist, JsonRequestBehavior.AllowGet);
              }
              else
              {
                  stafflist = _db.Staffs.Where(x => x.Username.StartsWith(SearchValue)).ToList();
                  return Json(stafflist, JsonRequestBehavior.AllowGet);
              }
          }



        public string GetRandomString(int seed)
        {
            //use the following string to control your set of alphabetic characters to choose from
            //for example, you could include uppercase too
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";


            // Random is not truly random,
            // so we try to encourage better randomness by always changing the seed value
            Random rnd = new Random((seed + DateTime.Now.Millisecond));


            // basic 5 digit random number
            string result = rnd.Next(10000, 99999).ToString();
           

            // single random character in ascii range a-z
            string alphaChar = alphabet.Substring(rnd.Next(0, alphabet.Length - 1), 1);


            // random position to put the alpha character
            int replacementIndex = rnd.Next(0, (result.Length - 1));
            result = result.Remove(replacementIndex, 1).Insert(replacementIndex, alphaChar);


            return result;
        }

        private char RandomNumber(int v1, int v2)
        {
            throw new NotImplementedException();
        }

    
        // GET: Staff/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

     


        // GET: Staff/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        public ActionResult Create(Staff s)
        {
            try
            {
                Staff n = new Staff();
                n.Name = s.Name;
                n.Username = s.Username;
                n.Email = s.Email;
                n.Phone = s.Phone;
                n.Address = s.Address;

                _db.Staffs.Add(n);
                _db.SaveChanges();
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
       
        // GET: Staff/Edit/5
        //   public ActionResult Edit(int id)
        public ActionResult Edit(string id)
        {
            Staff selectedStaff = _db.Staffs.Find(id);//_db.Staffs.Where(x => x.Email == id).First();

            return PartialView("_editStaff",selectedStaff);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        public ActionResult Edit(string id,Staff collection)
        {
            try
            {
                // TODO: Add update logic here
                Staff s = _db.Staffs.Find(id);//_db.Staffs.Where(x => x.Email == id).First();
                s.Name = collection.Name;
                s.Username = collection.Username;
             //   s.Email = collection.Email;
                s.Phone = collection.Phone;
                s.Address = collection.Address;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return PartialView("_editStaff");
            }
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Staff/Delete/5
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
