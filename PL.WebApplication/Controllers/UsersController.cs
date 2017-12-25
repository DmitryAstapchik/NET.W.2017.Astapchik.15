using PL.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DAL.Interface;
using Ninject;
using DependencyResolver;

namespace PL.WebApplication.Controllers
{
    public class UsersController : Controller
    {
        static IUsersRepository repository;

        static UsersController()
        {
            var kernel = new StandardKernel();
            kernel.ConfigurateResolver();
            repository = kernel.Get<IUsersRepository>();
        }


        [HttpGet]
       // [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
       // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginData data, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = repository.GetByEmail(data.Email);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(data.Email), "incorrect e-mail");
                    return View();
                }
                else if (user.Password != data.Password)
                {
                    ModelState.AddModelError(nameof(data.Password), "Incorrect password.");
                    return View();
                }
                else
                {
                    //User.
                    FormsAuthentication.SetAuthCookie(data.Email, data.Remember);
                    //Управляет службами проверки подлинности с помощью форм для веб-приложений
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
       // [AllowAnonymous]
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

                repository.Create(new UserDTO { Email = data.Email, FullName = data.FullName, Password = data.Password });
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