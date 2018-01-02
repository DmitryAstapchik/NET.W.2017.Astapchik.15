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
using PL.WebApplication.DAL.Interface;

namespace PL.WebApplication.Controllers
{
    //[HandleError(ExceptionType = typeof(Exception), View = "Error")]
    [Authenticate]
    public class ServiceController : Controller
    {
        private IAccountService service;
        IUsersRepository usersRepo;
        //BLL.Interface.AccountOwner user;

        public ServiceController(IUsersRepository repo)
        {
            var kernel = new StandardKernel();
            kernel.ConfigurateResolver();
            service = kernel.Get<IAccountService>();
            usersRepo = repo;
            //user = usersRepo.GetByEmail(User.Identity.Name);
        }

        //static ServiceController()
        //{
        //    var kernel = new StandardKernel();
        //    kernel.ConfigurateResolver();
        //    service = kernel.Get<IAccountService>();
        //}


        public ActionResult Index()
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);
            return this.View();
        }

        public ActionResult Deposit()
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);
            var accounts = service.GetPersonalAccounts(user).Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("deposit")]
        [HttpPost]
        public ActionResult DepositTo(Models.BankAccount account)
        {
            return View("DepositTo", account);
        }

        [HttpPost]
        public ActionResult MakeDeposit(Models.BankAccount account, decimal? amount)
        {
            if (!amount.HasValue || amount <= 0)
            {
                ModelState.AddModelError("deposit amount", "deposit amount must be a positive number");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    service.MakeDeposit(account.IBAN, amount.Value);
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                if (ModelState.IsValid)
                {
                    account = service.GetPersonalAccounts(usersRepo.GetByEmail(User.Identity.Name)).Single(a => a.IBAN == account.IBAN);
                    var values = new RouteValueDictionary(account)
                    {
                        { "amount", amount }
                    };
                    return RedirectToAction("DepositMade", values);
                }

            }

            return View("DepositTo", account);
        }

        public ViewResult DepositMade(Models.BankAccount account, decimal amount)
        {
            ViewBag.Amount = amount;
            return View(account);
        }

        [HttpGet]
        public ActionResult Withdraw()
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);
            var accounts = service.GetPersonalAccounts(user).Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("withdraw")]
        [HttpPost]
        public ViewResult WithdrawFrom(Models.BankAccount account)
        {
            return View("WithdrawFrom", account);
        }

        [HttpPost]
        public ActionResult MakeWithdrawal(Models.BankAccount account, decimal? amount)
        {
            if (!amount.HasValue || amount <= 0)
            {
                ModelState.AddModelError("withdrawal amount", "withdrawal amount must be a positive number");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    service.MakeWithdrawal(account.IBAN, amount.Value);
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                if (ModelState.IsValid)
                {
                    account = service.GetPersonalAccounts(usersRepo.GetByEmail(User.Identity.Name)).Single(a => a.IBAN == account.IBAN);
                    var values = new RouteValueDictionary(account)
                    {
                        { "amount" , amount }
                    };
                    return RedirectToAction("WithdrawalMade", values);
                }
            }

            return View("WithdrawFrom", account);
        }

        public ViewResult WithdrawalMade(Models.BankAccount account, decimal amount)
        {
            ViewBag.Amount = amount;
            return View(account);
        }

        [HttpGet]
        public ActionResult Transfer()
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);

            var accounts = service.GetPersonalAccounts(user).Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("transfer")]
        [HttpPost]
        public ActionResult Transfer(string from, string to)
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);

            var acc1 = service.GetPersonalAccounts(user).Single(a => a.IBAN == from);
            var acc2 = service.GetPersonalAccounts(user).Single(a => a.IBAN == to);
            return View("TransferBetween", new[] { acc1, (Models.BankAccount)acc2 });
        }

        [HttpPost]
        public ActionResult MakeTransfer(string from, string to, decimal amount)
        {
            service.MakeWithdrawal(from, amount);
            service.MakeDeposit(to, amount);
            var user = usersRepo.GetByEmail(User.Identity.Name);

            var acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == to);
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
        public ActionResult CreateAccount(Models.NewAccount account)
        {
            if (account.StartBalance < (int)account.Type)
            {
                ModelState.AddModelError(nameof(account.StartBalance), $"minimal start balance amount is {(int)account.Type}");
            }

            //if (string.IsNullOrWhiteSpace(account.Owner))
            //{
            //    ModelState.AddModelError("owner", "owner cannot be empty or white spaces only");
            //}

            if (ModelState.IsValid)
            {
                var user = usersRepo.GetByEmail(User.Identity.Name);

                var iban = service.OpenAccount(user, account.StartBalance);
                var acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == iban);
                //var dic = new RouteValueDictionary { { "account", (Models.BankAccount)acc }, { "owner", (Models.AccountOwner)acc.Owner } };
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
            var user = usersRepo.GetByEmail(User.Identity.Name);

            var accounts = service.GetPersonalAccounts(user).Select(a => (Models.BankAccount)a);
            return View(accounts);
        }

        [ActionName("close")]
        [HttpPost]
        public ActionResult CloseAccount(Models.BankAccount account)
        {
            //var user = usersRepo.GetByEmail(User.Identity.Name);

            //var acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == account.IBAN);
            service.CloseAccount(account.IBAN);
            return RedirectToAction("Closed", account);
        }

        public ViewResult Closed(Models.BankAccount account)
        {
            return View(account);
        }

        [ActionName("AccountsList")]
        public ViewResult DisplayAccountsList()
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);

            var accounts = service.GetPersonalAccounts(user);
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