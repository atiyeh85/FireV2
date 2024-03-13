using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using FireV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FireV2.Utility
{
    public static class UAC
    {
        public static bool Validate(System.Security.Principal.IPrincipal User, string action)
        {
            if (!User.Identity.IsAuthenticated)
                return false;
            //if (User.Identity.IsAuthenticated && User.Identity.Name == "Admin")
            //    return true;

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //var RoleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            //if (User.IsInRole("Admin"))
            //    return true;

            using (var db = new StoreDb ())
            {
                try
                {
                    return db.UserActions.Any(u => u.UserName.ToLower() == User.Identity.Name.ToLower() && u.MyAction.ActionName.ToLower() == action.ToLower());
                }
                catch (Exception ex)
                {
                }
            }

            return false;
        }
        public static bool Validate(System.Security.Principal.IPrincipal User)
        {
            if (!User.Identity.IsAuthenticated)
                return false;
           
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var usr = UserManager.FindByName(User.Identity.Name);
            string[] roles = UserManager.GetRoles(usr.Id).ToArray();
            using (var db = new FireV2.Models.StoreDb())
            {
                foreach (string role in roles)
                {
                    if (role == "Admin")
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        public static bool IsRestrictedWorkDays(System.Security.Principal.IPrincipal User)
        {
            if (!User.Identity.IsAuthenticated)
                return false;
            if (User.Identity.IsAuthenticated && User.Identity.Name == "admin")
                return true;
            if (bool.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["RestrictedWorkDays"]))
            {
                int start = int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["StartWorkDay"]);
                int end = int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["EndWorkDay"]);

                System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

                return (p.GetDayOfMonth(DateTime.Now) >= start && p.GetDayOfMonth(DateTime.Now) <= end);
            }

            return true;
        }
    }
}