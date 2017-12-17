using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface;

namespace PL.WebApplication.Controllers
{
    public class CreateController : Controller
    {
        static IAccountService service = HomeController.service;

        // GET: Create
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string owner, string balance)
        {
            var iban = service.OpenAccount(owner, decimal.Parse(balance));
            var account = service.GetAllAccounts().Single(acc => acc.IBAN == iban);
            return View("Created", account);
        }
    }
}