using PL.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using PL.Web.DAL.Interface;

namespace PL.Web.Controllers
{
    public class UsersController : Controller
    {
        IUsersRepository repository;

        public UsersController(IUsersRepository repo)
        {
            this.repository = repo;
        }


        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginData data, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = repository.GetByEmail(data.Email);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(data.Email), "not registered e-mail");
                    return View();
                }
                else if (!Crypto.VerifyHashedPassword( user.Password, data.Password))
                {
                    ModelState.AddModelError(nameof(data.Password), "Incorrect password.");
                    return View();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(data.Email, data.Remember);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Service");
                    }
                }
            }
            return View(data);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationData data)
        {
            if (ModelState.IsValid)
            {
                if (repository.GetByEmail(data.Email) != null)
                {
                    ModelState.AddModelError(nameof(data.Email), "User with this address already registered.");
                    return View(data);
                }

                repository.Create(new UserDTO { Email = data.Email, FullName = data.FullName, Password = Crypto.HashPassword(data.Password), PassportID = data.PassportID });
                FormsAuthentication.SetAuthCookie(data.Email, false);
                return RedirectToAction("Index", "Service");
            }

            return View(data);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "users");
        }
    }
}