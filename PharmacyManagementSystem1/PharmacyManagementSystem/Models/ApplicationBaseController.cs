using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PharmacyManagementSystem.Models;
using Microsoft.AspNet.Identity;

namespace PharmacyManagementSystem.Models
{
    public class ApplicationBaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                var context = new PharmacyDBEntities2();
                var userid = User.Identity.GetUserId();
                var username = context.AspNetUsers.Where(x => x.Id == userid).First().AccountUserName;
                var currentdate = DateTime.Today.ToString("yyyy-MM-dd");
                var MedicineAddedToday = context.Stocks.Where(x => x.AddedDate.ToString() == currentdate).Count();
                if (!string.IsNullOrEmpty(username) || MedicineAddedToday>=0)
                {
                    ViewData.Add("FullName", username);
                    ViewData.Add("MedicineAddToday", MedicineAddedToday);

                }
            }
            base.OnActionExecuted(filterContext);
        }
        public ApplicationBaseController()
        { }
    }
}