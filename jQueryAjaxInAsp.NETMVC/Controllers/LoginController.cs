using jQueryAjaxInAsp.NETMVC.Models;
using jQueryAjaxInAsp.NETMVC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQueryAjaxInAsp.NETMVC.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult Autherize(jQueryAjaxInAsp.NETMVC.Models.ViewModel.UserModel userModel)
        {
            using (Entities db = new Entities())
            {
                var userDetails = db.Users.Where(x => x.UserName == userModel.UserName && x.Password == userModel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                else {
                    var Rolename = db.Roles.Where(x => x.Id == userDetails.RoleId).FirstOrDefault().RoleName;
                    Session["userID"] = userDetails.UserID;
                    Session["userName"] = userDetails.UserName;
                    Session["roleName"] = Rolename;                
                    if (Rolename == "admin")
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                    if (Rolename == "employee")
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        userModel.LoginErrorMessage = "Something went wrong.";
                        return View("Index", userModel);
                    }
                }
            }
        }

        public ActionResult LogOut()
        {
            int userId = (int)Session["userID"];
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }
        
	}
}