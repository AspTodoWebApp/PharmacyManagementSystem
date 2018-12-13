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
using System.Threading.Tasks;

namespace PharmacyManagementSystem.Controllers
{
    [Authorize(Roles="Admin")]
    public class StaffController : ApplicationBaseController
    {
        PharmacyDBEntities4 _db;
      
        public  StaffController()
        {
            _db = new PharmacyDBEntities4();
        }
        // GET: Staff

        // [Authorize(Roles ="Manager")]
     //   [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
          return View(_db.Staffs.ToList());
         
        }
        public JsonResult GetSearchValue(string search)
        {
           
            List<Staff> allsearch = _db.Staffs.Where(x => x.Email.StartsWith(search)).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
        //  [Authorize(Roles ="Admin")]
    //    [Authorize(Roles="Admin")]
        public ActionResult addStaff()
        {
            
            return PartialView("_addStaff");
         
        } 
        [HttpPost]
    
        public ActionResult addStaff(Staff s)
        {
            try
            {
            Staff st = new Staff();
                st.Id = Guid.NewGuid().ToString();
                st.Name = s.Name;
                st.Username = s.Username;
                st.Email = s.Email;
                st.Phone = s.Phone;
                st.Address = s.Address;
                _db.Staffs.Add(st);

                
                string random = GetRandomString(5);
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                   client.EnableSsl = true;
                   //  client.Timeout = 1000000;
                   client.DeliveryMethod = SmtpDeliveryMethod.Network;
                   client.UseDefaultCredentials = false;
                   client.Credentials = new NetworkCredential("numanuet311@gmail.com", "47841271");
                   MailMessage msg = new MailMessage();
                   msg.To.Add(s.Email);
                   msg.From = new MailAddress("numanuet311@gmail.com");
                   msg.Subject = "Staff added by admin";
                   msg.Body = "You have registered as staff in pharmacy system your login ID is : "+s.Username+" and password is : "+random;
                   client.Send(msg);
                //     random = "Numan311@";
                  var hasher = new PasswordHasher();
                       AspNetUser staff = new AspNetUser();
                     staff.Id = Guid.NewGuid().ToString();
                       staff.Email = s.Username;
                     staff.EmailConfirmed = false;
                staff.PhoneNumberConfirmed = false;
                     staff.TwoFactorEnabled = false;
                     staff.LockoutEnabled = true;
                     staff.AccountUserName = s.Name;
                     staff.AccessFailedCount = 0;
                     staff.GmailAccount = s.Email;
                       staff.SecurityStamp =Guid.NewGuid().ToString();
                     staff.PasswordHash = hasher.HashPassword(random);
                         staff.UserName = s.Username;
                     _db.AspNetUsers.Add(staff);

                /*  AspNetRole staffRole = new AspNetRole();
                  staffRole.Id = Guid.NewGuid().ToString();
                  staffRole.Name = "Staff";
                  staffRole.Discriminator = "ApplicationRole";
                  _db.AspNetRoles.Add(staffRole);*/

                   AspNetUserRole assignRole = new AspNetUserRole();
                    assignRole.AssignRoleId = Guid.NewGuid().ToString();
                    assignRole.RoleId = _db.AspNetRoles.Where(role => role.Name == "Staff").FirstOrDefault().Id;
                    assignRole.UserId = staff.Id;
                    _db.AspNetUserRoles.Add(assignRole);

                _db.SaveChanges();




                TempData["msg"] = "<script>alert('added');</script>";

                return RedirectToAction("Index");

            }
            catch
            {
                return RedirectToAction("Create");
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
        public ActionResult Edit(string Id)
        {
            Staff selectedStaff = _db.Staffs.Where(x => x.Email == Id).First();

            return PartialView("_editStaff",selectedStaff);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        public ActionResult Edit(string Id,Staff collection)
        {
            try
            {
                // TODO: Add update logic here
                Staff s = _db.Staffs.Find(Id);//_db.Staffs.Where(x => x.Email == id).First();
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
        public ActionResult Delete(string id)
        {//remove from staff table
         // string Id = id + "@gmail.com";

            Staff selectedStaff = _db.Staffs.Find(id);
            _db.Staffs.Remove(selectedStaff);

            //remove staff role and login 
            AspNetUser user = _db.AspNetUsers.Where(obj => obj.GmailAccount == selectedStaff.Email).First();
            AspNetUserRole userRole= _db.AspNetUserRoles.Where(obj => obj.UserId == user.Id).First();
           _db.AspNetUsers.Remove(user);
            _db.AspNetUserRoles.Remove(userRole);
            _db.SaveChanges();
            return RedirectToAction("Index");
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
