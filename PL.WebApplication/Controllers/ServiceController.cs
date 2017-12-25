using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BLL.Interface;
using DependencyResolver;
using Ninject;
using PL.WebApplication.Filters;
using PL.WebApplication.Models;
using System.Web.Security;
using System.Security.Principal;

namespace PL.WebApplication.Controllers
{
    //[HandleError(ExceptionType = typeof(Exception), View = "Error")]
     [Authenticate]
    public class ServiceController : Controller
    {
        private static IAccountService service;
        string user;

        static ServiceController()
        {
            var kernel = new StandardKernel();
            kernel.ConfigurateResolver();
            service = kernel.Get<IAccountService>();
        }

        public ServiceController()
        {
           
        }

        //[Authenticate]
        public ActionResult Index()
        {
            user = User.Identity.Name;
            return this.View();
        }

      

        public ActionResult Deposit()
        {
            var accounts = service.GetUserAccounts(User.Identity.Name).Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("deposit")]
        [HttpPost]
        public ActionResult DepositTo(Models.BankAccount account)
        {
            return View("DepositTo", account);
        }

        [HttpPost]
        public ActionResult MakeDeposit(Models.BankAccount account, decimal amount)
        {
            var newBalance = service.MakeDeposit(account.IBAN, amount);
            account.Balance = newBalance;
            var values = new RouteValueDictionary(account)
            {
                { "amount", amount }
            };
            return RedirectToAction("DepositMade", values);
        }

        public ViewResult DepositMade(Models.BankAccount account, decimal amount)
        {
            ViewBag.Amount = amount;
            return View(account);
        }

        [HttpGet]
        public ActionResult Withdraw()
        {
            var accounts = service.GetUserAccounts(User.Identity.Name).Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("withdraw")]
        [HttpPost]
        public ViewResult WithdrawFrom(Models.BankAccount account)
        {
            return View("WithdrawFrom", account);
        }

        [HttpPost]
        public RedirectToRouteResult MakeWithdrawal(Models.BankAccount account, decimal amount)
        {
            var newBalance = service.MakeWithdrawal(account.IBAN, amount);
            ViewBag.Amount = amount;
            account.Balance = newBalance;
            var values = new RouteValueDictionary(account)
            {
                { "amount" , amount }
            };
            return RedirectToAction("WithdrawalMade", values);
        }

        public ViewResult WithdrawalMade(Models.BankAccount account, decimal amount)
        {
            ViewBag.Amount = amount;
            return View(account);
        }

        [HttpGet]
        public ActionResult Transfer()
        {
            var accounts = service.GetUserAccounts(User.Identity.Name).Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("transfer")]
        [HttpPost]
        public ActionResult Transfer(string from, string to)
        {
            var acc1 = service.GetUserAccounts(User.Identity.Name).Single(a => a.IBAN == from);
            var acc2 = service.GetUserAccounts(User.Identity.Name).Single(a => a.IBAN == to);
            return View("TransferBetween", new[] { (Models.BankAccount)acc1, (Models.BankAccount)acc2 });
        }

        [HttpPost]
        public ActionResult MakeTransfer(string from, string to, decimal amount)
        {
            service.MakeWithdrawal(from, amount);
            service.MakeDeposit(to, amount);
            var acc = service.GetUserAccounts(User.Identity.Name).Single(a => a.IBAN == to);
            var values = new RouteValueDictionary(acc) { { "amount", amount } };
            return RedirectToAction("TransferMade", values);
        }

        public ViewResult TransferMade(Models.BankAccount to, decimal amount)
        {
            ViewBag.Amount = amount;
            return View(to);
        }

        [ActionName("create")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [ActionName("create")]
        [HttpPost]
        public ActionResult CreateAccount([Bind(Include = "Type, Balance ")]Models.BankAccount account)
        {
            if (account.Balance < (int)account.Type)
            {
                ModelState.AddModelError("balance", $"minimal start balance amount is {(int)account.Type}");
            }

            //if (string.IsNullOrWhiteSpace(account.Owner))
            //{
            //    ModelState.AddModelError("owner", "owner cannot be empty or white spaces only");
            //}

            if (ModelState.IsValid)
            {
                var iban = service.OpenAccount(account.Owner, account.Balance);
                var acc = service.GetUserAccounts(User.Identity.Name).Single(a => a.IBAN == iban);
                return RedirectToAction("Created", (Models.BankAccount)acc);
            }

            return View();
        }

        public ViewResult Created(Models.BankAccount account)
        {
            return View(account);
        }

        [ActionName("close")]
        public ActionResult CloseAccount()
        {
            var accounts = service.GetUserAccounts(User.Identity.Name).Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("close")]
        [HttpPost]
        public ActionResult CloseAccount(string iban)
        {
            var account = service.GetUserAccounts(User.Identity.Name).Single(a => a.IBAN == iban);
            service.CloseAccount(iban);
            return RedirectToAction("Closed", account);
        }

        public ViewResult Closed(Models.BankAccount account)
        {
            return View(account);
        }

        [ActionName("AccountsList")]
        public ViewResult DisplayAccountsList()
        {
            var accounts = service.GetUserAccounts(User.Identity.Name);
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