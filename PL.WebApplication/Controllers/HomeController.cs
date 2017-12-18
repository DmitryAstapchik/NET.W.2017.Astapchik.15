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

        public ActionResult Deposit()
        {
            var accounts = service.GetAllAccounts().Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        public ActionResult DepositTo(string iban)
        {
            var account = (Models.BankAccount)service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return View(account);
        }

        [HttpPost]
        public ActionResult MakeDeposit(string iban, decimal amount)
        {
            service.MakeDeposit(iban, amount);
            ViewBag.Amount = amount;
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return RedirectToAction("DepositMade", new { iban = iban, amount = amount });
        }

        public ViewResult DepositMade(string iban, decimal amount)
        {
            var acc = (Models.BankAccount)service.GetAllAccounts().Single(a => a.IBAN == iban);
            ViewBag.Amount = amount;
            return View(acc);
        }

        public ActionResult Withdraw()
        {
            var accounts = service.GetAllAccounts().Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [HttpPost]
        public ViewResult WithdrawFrom(string iban)
        {
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return View((Models.BankAccount)account);
        }

        [HttpPost]
        public RedirectToRouteResult MakeWithdrawal(string iban, decimal amount)
        {
            service.MakeWithdrawal(iban, amount);
            ViewBag.Amount = amount;
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            TempData["amount"] = amount;
            return RedirectToAction("WithdrawalMade", (Models.BankAccount)account);
        }

        public ViewResult WithdrawalMade(Models.BankAccount account, decimal? amount)
        {
            amount = (decimal)TempData["amount"];
            ViewBag.Amount = amount;
            return View(account);
        }

        [ActionName("create")]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [ActionName("create")]
        [HttpPost]
        public RedirectToRouteResult CreateAccount(string owner, string balance)
        {
            var iban = service.OpenAccount(owner, decimal.Parse(balance));
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return RedirectToAction("Created", account);
        }

        public ViewResult Created(Models.BankAccount account)
        {
            return View(account);
        }

        [ActionName("close")]
        public ActionResult CloseAccount()
        {
            var accounts = service.GetAllAccounts().Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("close")]
        [HttpPost]
        public ActionResult CloseAccount(string iban)
        {
            var account = service.GetAllAccounts().Single(a => a.IBAN == iban);
            service.CloseAccount(iban);
            return RedirectToAction("Closed", account);
        }

        public ViewResult Closed(Models.BankAccount account)
        {
            return View(account);
        }

        [ActionName("AccountList")]
        public ViewResult DisplayAccountList()
        {
            var accounts = service.GetAllAccounts();
            return View(accounts.Select(a => (Models.BankAccount)a));
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