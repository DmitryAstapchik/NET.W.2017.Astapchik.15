using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
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
            var accounts = service.GetAllAccounts().Select(a => (Models.BankAccount)a);
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
            var accounts = service.GetAllAccounts().Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("transfer")]
        [HttpPost]
        public ActionResult Transfer(string from, string to)
        {
            var acc1 = service.GetAllAccounts().Single(a => a.IBAN == from);
            var acc2 = service.GetAllAccounts().Single(a => a.IBAN == to);
            return View("TransferBetween", new[] { (Models.BankAccount)acc1, (Models.BankAccount)acc2 });
        }

        [HttpPost]
        public ActionResult MakeTransfer(string from, string to, decimal amount)
        {
            service.MakeWithdrawal(from, amount);
            service.MakeDeposit(to, amount);
            var acc = service.GetAllAccounts().Single(a => a.IBAN == to);
            var values = new RouteValueDictionary(acc) { { "amount", amount } };
            return RedirectToAction("TransferMade", values);
        }

        public ViewResult TransferMade(Models.BankAccount to, decimal amount)
        {
            ViewBag.Amount = amount;
            return View(to);
        }

        [ActionName("create")]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [ActionName("create")]
        [HttpPost]
        public RedirectToRouteResult CreateAccount(string owner, decimal balance)
        {
            var iban = service.OpenAccount(owner, balance);
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return RedirectToAction("Created", (Models.BankAccount)account);
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