using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using HomeWork3.Models;
using HomeWork3Data;
using HomeWork3Data.DataModel;
using System.Web.Security;
using HomeWork3Data.DataModel;
using HomeWork3Common.Helpers.Security;
using HomeWork3Business.Interfaces;

namespace HomeWork3.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userservice;

        public AccountController(IUserService userservice, IUserRepository userRepository)
        {
            _userservice = userservice;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Now if our password was enctypted or hashed we would have done the
                // same operation on the user entered password here, But for now
                // since the password is in plain text lets just authenticate directly

                var LogUser = _userservice.Contains(model.UserName, model.Password);
                if (LogUser != null)
                {
                    // * !!! *
                    // Creating a FromsAuthenticationTicket is what 
                    // will set RequestContext.HttpContext.Request.IsAuthenticated to True
                    // in the AdminAuthorize attribute code below
                    // * !!! *
                    var ticket = new FormsAuthenticationTicket(1, // version 
                                                               model.UserName, // user name
                                                               DateTime.Now, // create time
                                                               DateTime.Now.AddMinutes(30), // expire time
                                                               true, // persistent
                                                               LogUser.Role.TrimEnd(), // user data, such as roles
                                                               FormsAuthentication.FormsCookiePath);

                    var strEncryptedTicket = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strEncryptedTicket);
                    Response.Cookies.Add(cookie);

                    // Redirect back to the page you were trying to access
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }

                // If we got this far, something failed, redisplay form
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}