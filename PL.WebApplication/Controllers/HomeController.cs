using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface;
using DependencyResolver;
using Ninject;

namespace PL.WebApplication.Controllers
{
    //[HandleError(ExceptionType = typeof(Exception), View = "Error")]
    public class HomeController : Controller
    {
        internal static IAccountService service;

        static HomeController()
        {
            var kernel = new StandardKernel();
            kernel.ConfigurateResolver();
            service = kernel.Get<IAccountService>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult ViewAccountList()
        {
            var accounts = service.GetAllAccounts();
            return View(accounts);
        }

        public ViewResult ViewAccountInfo(string iban)
        {
            var acc = service.GetAllAccounts().Single(a => a.IBAN == iban);
            return View("AccountInfo", acc);
        }

        //[ChildActionOnly]
        public ViewResult Error()
        {
            return View("Error");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}