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
        private static IAccountService service;
        IUsersRepository usersRepo;
        //BLL.Interface.AccountOwner user;

        public ServiceController(IUsersRepository repo)
        {
            usersRepo = repo;
        }

        static ServiceController()
        {
            var kernel = new StandardKernel();
            kernel.Unbind<ModelValidatorProvider>();
            kernel.ConfigurateResolver();
            service = kernel.Get<IAccountService>();
        }

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

        // [ActionName("deposit")]
        [HttpPost]
        public ActionResult DepositTo(string iban)
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);
            Models.BankAccount acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == iban);
            return View("DepositTo", acc);
        }

        [HttpPost]
        public ActionResult MakeDeposit(string iban, decimal amount)
        {
            //if (!amount.HasValue || amount <= 0)
            //{
            //    ModelState.AddModelError("deposit amount", "deposit amount must be a positive number");
            //}

            //if (ModelState.IsValid)
            //{
            try
            {
                service.MakeDeposit(iban, amount);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            Models.BankAccount account = service.GetPersonalAccounts(usersRepo.GetByEmail(User.Identity.Name)).Single(a => a.IBAN == iban);
            if (ModelState.IsValid)
            {
                account = service.GetPersonalAccounts(usersRepo.GetByEmail(User.Identity.Name)).Single(a => a.IBAN == iban);
                var values = new RouteValueDictionary(account)
                    {
                        { "amount", amount }
                    };
                return RedirectToAction("DepositMade", values);
            }

            //}

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

        // [ActionName("withdraw")]
        [HttpPost]
        public ViewResult WithdrawFrom(string iban)
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);
            Models.BankAccount acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == iban);
            return View("WithdrawFrom", acc);
        }

        [HttpPost]
        public ActionResult MakeWithdrawal(string iban, decimal amount)
        {
            //if (!amount.HasValue || amount <= 0)
            //{
            //    ModelState.AddModelError("withdrawal amount", "withdrawal amount must be a positive number");
            //}

            //if (ModelState.IsValid)
            //{
            Models.BankAccount account = service.GetPersonalAccounts(usersRepo.GetByEmail(User.Identity.Name)).Single(a => a.IBAN == iban);
            try
            {
                service.MakeWithdrawal(iban, amount);
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
            //}

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

        // [ActionName("transfer")]
        [HttpPost]
        public ActionResult TransferToMy(string fromIBAN)
        {
            //var user = usersRepo.GetByEmail(User.Identity.Name);
            //var acc1 = service.GetPersonalAccounts(user).Single(a => a.IBAN == from);
            //var acc2 = service.GetPersonalAccounts(user).Single(a => a.IBAN == to);
            //return View("TransferBetween", new[] { acc1, (Models.BankAccount)acc2 });

            var user = usersRepo.GetByEmail(User.Identity.Name);
            ViewBag.Accounts = service.GetPersonalAccounts(user).Where(a => a.IBAN != fromIBAN).Select(a => (Models.BankAccount)a).ToArray();
            Models.BankAccount acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == fromIBAN);
            return View(acc);
        }

        [HttpPost]
        public ActionResult TransferToMyAmount(string fromIBAN, string toIBAN)
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);
            var from = service.GetPersonalAccounts(user).Single(a => a.IBAN == fromIBAN);
            var to = service.GetPersonalAccounts(user).Single(a => a.IBAN == toIBAN);
            return View(new[] { from, (Models.BankAccount)to });
        }

        [HttpPost]
        public ActionResult TransferToAnothers(string fromIBAN)
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);
            //ViewBag.Accounts = service.GetPersonalAccounts(user).Where(a => a.IBAN != fromIBAN).Select(a => (Models.BankAccount)a).ToArray();
            Models.BankAccount acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == fromIBAN);
            return View(acc);
        }

        public ActionResult GetAnothersAccount(string iban, string fromIBAN)
        {
            var acc = service.GetAccount(iban);
            if (acc == null)
            {
                return Content($"<i class='text-danger'>IBAN {iban} was not found.</i>");
            }
            else
            {
                if (acc.Owner.Email == User.Identity.Name)
                {
                    return Content("<i class='text-danger'>It is your own account.</i>");
                }
                else
                {
                    ViewBag.toIBAN = iban;
                    ViewBag.fromIBAN = fromIBAN;
                    return PartialView("_TransferToAnothersAmount", acc.Owner.FullName);
                }
            }
        }

        [HttpPost]
        public ActionResult MakeTransfer(string fromIBAN, string toIBAN, decimal amount)
        {
            //service.MakeWithdrawal(fromIBAN, amount);
            //service.MakeDeposit(toIBAN, amount);
            service.MakeTransfer(fromIBAN, toIBAN, amount);

            var user = usersRepo.GetByEmail(User.Identity.Name);

            Models.BankAccount acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == fromIBAN);
            var values = new RouteValueDictionary(acc) { { "amount", amount } };
            return RedirectToAction("TransferMade", values);
        }

        public ViewResult TransferMade(Models.BankAccount from, decimal amount)
        {
            ViewBag.Amount = amount;
            return View(from);
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
            if (ModelState.IsValid)
            {
                var user = usersRepo.GetByEmail(User.Identity.Name);
                var iban = service.OpenAccount(user, account.StartBalance);
                var acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == iban);
                return RedirectToAction("Created", (Models.BankAccount)acc);
            }

            return View(account);
        }

        [HttpPost]
        public PartialViewResult GetAccountModal(string iban)
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);
            Models.BankAccount acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == iban);
            return PartialView("_AccountModal", acc);
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
        public ActionResult CloseAccount(string iban)
        {
            var user = usersRepo.GetByEmail(User.Identity.Name);
            Models.BankAccount acc = service.GetPersonalAccounts(user).Single(a => a.IBAN == iban);
            service.CloseAccount(iban);
            return RedirectToAction("Closed", acc);
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