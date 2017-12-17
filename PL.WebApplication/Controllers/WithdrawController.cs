using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface;

namespace PL.WebApplication.Controllers
{
    public class WithdrawController : Controller
    {
        static IAccountService service = HomeController.service;

        // GET: Withdraw
        public ActionResult Index()
        {
            var accounts = HomeController.service.GetAllAccounts();
            ViewData["controller"] = "Withdraw";
            ViewData["action"] = "WithdrawFrom";
            ViewData["command"] = "Withdraw";
            return View(accounts);
        }

       // [ChildActionOnly]
        public ViewResult WithdrawFrom(string iban)
        {
            //service.MakeWithdrawal(iban, amount);
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return View(account);
        }

        //[ChildActionOnly]
        public ViewResult MakeWithdrawal(string iban, decimal amount)
        {
            service.MakeWithdrawal(iban, amount);
            ViewBag.Amount = amount;
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return View(account);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            //var model = new HandleErrorInfo(filterContext.Exception, "Withdraw", "Action");

            //filterContext.Result = new ViewResult()
            //{
            //    ViewName = "Error",
            //    ViewData = new ViewDataDictionary(model)
            //};

            filterContext.Result = View("Error", ex);
        }
    }
}