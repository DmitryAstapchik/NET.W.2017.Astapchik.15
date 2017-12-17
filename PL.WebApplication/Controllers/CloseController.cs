using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface;

namespace PL.WebApplication.Controllers
{
    public class CloseController : Controller
    {
        static IAccountService service = HomeController.service;
        // GET: Close
        public ActionResult Index()
        {
            var accounts = service.GetAllAccounts();
            ViewData["controller"] = "Close";
            ViewData["action"] = "Index";
            ViewData["command"] = "Close";
            return View(accounts);
        }

        [HttpPost]
        public ActionResult Index(string iban)
        {
            var acc = service.GetAllAccounts().Single(a => a.IBAN == iban);
            service.CloseAccount(iban);
            return View("Closed", acc);
        }
    }
}