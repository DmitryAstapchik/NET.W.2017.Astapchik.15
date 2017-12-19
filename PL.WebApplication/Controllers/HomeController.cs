﻿using System;
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

        public ActionResult DepositTo(Models.BankAccount account)
        {
            return View(account);
        }

        [HttpPost]
        public ActionResult MakeDeposit(Models.BankAccount account, decimal amount)
        {
            service.MakeDeposit(account.IBAN, amount);
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

        public ActionResult Withdraw()
        {
            var accounts = service.GetAllAccounts().Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        public ViewResult WithdrawFrom(Models.BankAccount account)
        {
            return View(account);
        }

        [HttpPost]
        public RedirectToRouteResult MakeWithdrawal(Models.BankAccount account, decimal amount)
        {
            service.MakeWithdrawal(account.IBAN, amount);
            ViewBag.Amount = amount;
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