using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface;

namespace PL.WebApplication.Controllers
{
    public class DepositController : Controller
    {
        static IAccountService service = HomeController.service;

        // GET: Deposit
        public ActionResult Index()
        {
            var accounts = service.GetAllAccounts();
            ViewData["controller"] = "Deposit";
            ViewData["action"] = "DepositTo";
            ViewData["command"] = "Deposit";
            return View(accounts);
        }

        [HttpPost]
        public ActionResult DepositTo(string iban)
        {
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return View(account);
        }

        [HttpPost]
        public ActionResult MakeDeposit(string iban, decimal amount)
        {
            service.MakeDeposit(iban, amount);
            ViewBag.Amount = amount;
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return View(account);
            //return RedirectToAction("ViewAccountInfo", "Home", new { iban = iban });
        }
    }
}