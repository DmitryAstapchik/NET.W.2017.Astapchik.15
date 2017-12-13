using System;
using System.Linq;
using BLL.Interface;
using DependencyResolver;
using Ninject;

namespace ConsolePL
{
    internal class Program
    {
        private static readonly IKernel Resolver;

        static Program()
        {
            Resolver = new StandardKernel();
            Resolver.ConfigurateResolver();
        }

        private static void Main(string[] args)
        {
            IAccountService service = Resolver.Get<IAccountService>();

            Console.WriteLine("open account #1 with start balance 100");
            var iban1 = service.OpenAccount("Account owner 1", 100);
            Console.WriteLine("open account #2 with start balance 2000");
            var iban2 = service.OpenAccount("Account owner 2", 2000);
            Console.WriteLine("open account #3 with start balance 7000");
            var iban3 = service.OpenAccount("Account owner 3", 7000);

            var balance1 = service.MakeDeposit(iban1, 200);
            Console.WriteLine($"deposit 200 to account #1, new balance: {balance1}");
            balance1 = service.MakeDeposit(iban1, 400);
            Console.WriteLine($"deposit 400 to account #1, new balance: {balance1}");
            var balance2 = service.MakeDeposit(iban2, 5000);
            Console.WriteLine($"deposit 5000 to account #2, new balance: {balance2}");

            balance1 = service.MakeWithdrawal(iban1, 350);
            Console.WriteLine($"withdraw 350 from account #1, new balance: {balance1}");
            balance2 = service.MakeWithdrawal(iban2, 1700);
            Console.WriteLine($"withdraw 1700 from account #2, new balance: {balance2}");
            balance2 = service.MakeWithdrawal(iban2, 3900);
            Console.WriteLine($"withdraw 3900 from account #2, new balance: {balance2}");

            var balance3 = service.CloseAccount(iban3);
            Console.WriteLine($"close account #3, money retrieved: {balance3}");

            Console.Read();
        }
    }
}
