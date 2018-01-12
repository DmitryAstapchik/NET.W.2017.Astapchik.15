using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Interface;
using DAL.Interface;
using DAL.Interface.Interfaces;
using Moq;
using NUnit.Framework;

namespace BLL.Tests
{
    [TestFixture]
    public class BankTests
    {
        private static IBonusPointsCalculator calculator = Mock.Of<IBonusPointsCalculator>();
        private static IIBANGenerator generator = Mock.Of<IIBANGenerator>(g => g.GenerateIBAN() == "fake iban");
        private static Mock<BankAccountDTO> dtoMock = new Mock<BankAccountDTO>("iban", new AccountOwnerDTO("pid", "name", "email"), 22m, 0, BankAccountDTO.AccountType.StandardAccount);
        private static IAccountsRepository repo = Mock.Of<IAccountsRepository>(st => st.GetByIban(It.IsAny<string>()) == dtoMock.Object);
        private static IAccountsUnitOfWork uow = Mock.Of<IAccountsUnitOfWork>(u => u.Accounts == repo);
        private static Bank bank = new Bank(uow, generator, calculator);
        private static Mock<IAccountsRepository> repoMock = Mock.Get(repo);

        [Test]
        public void CloseAccountTest()
        {
            Assert.Throws<ArgumentException>(() => bank.CloseAccount(string.Empty));
            Assert.Throws<ArgumentException>(() => bank.CloseAccount(null));
            Assert.Throws<ArgumentException>(() => bank.CloseAccount(" "));

            bank.CloseAccount("iban");
            repoMock.Verify(r => r.Update(It.Is<BankAccountDTO>(a => a.Status == BankAccountDTO.AccountStatus.Inactive)));
        }

        [Test]
        public void MakeDepositTest()
        {
            Assert.Throws<ArgumentException>(() => bank.MakeDeposit(string.Empty, 200));
            Assert.Throws<ArgumentException>(() => bank.MakeDeposit(null, 200));
            Assert.Throws<ArgumentException>(() => bank.MakeDeposit(" ", 200));

            Assert.Throws<ArgumentException>(() => bank.MakeDeposit("55", 49));
            Assert.DoesNotThrow(() => bank.MakeDeposit("55", 50));

            Assert.AreEqual(122, bank.MakeDeposit("ss", 100));
            repoMock.Verify(st => st.Update(It.Is<BankAccountDTO>(dto => dto.Balance == 122)));
        }

        [Test]
        public void MakeWithdrawalTest()
        {
            Assert.Throws<ArgumentException>(() => bank.MakeWithdrawal(string.Empty, 22));
            Assert.Throws<ArgumentException>(() => bank.MakeWithdrawal(null, 22));
            Assert.Throws<ArgumentException>(() => bank.MakeWithdrawal(" ", 22));

            Assert.Throws<ArgumentException>(() => bank.MakeWithdrawal("iban", 9));
            Assert.DoesNotThrow(() => bank.MakeWithdrawal("55", 10));

            Assert.Throws<InvalidOperationException>(() => bank.MakeWithdrawal("ss", 23));

            Assert.AreEqual(0, bank.MakeWithdrawal("sss", 22));
            repoMock.Verify(st => st.Update(It.Is<BankAccountDTO>(dto => dto.Balance == 0)));
        }

        [Test]
        public void OpenAccountTest()
        {
            var owner = new AccountOwner("pid", "name", "email");

            Assert.Throws<ArgumentNullException>(() => bank.OpenAccount(null, 100));
            Assert.DoesNotThrow(() => bank.OpenAccount(owner, 50));

            bank.OpenAccount(owner, 1000);
            repoMock.Verify(st => st.Create(It.Is<BankAccountDTO>(acc => acc.Type.ToString() == typeof(StandardAccount).Name)));

            bank.OpenAccount(owner, 1001);
            repoMock.Verify(st => st.Create(It.Is<BankAccountDTO>(acc => acc.Type.ToString() == typeof(GoldAccount).Name)));

            bank.OpenAccount(owner, 10000);
            repoMock.Verify(st => st.Create(It.Is<BankAccountDTO>(acc => acc.Type.ToString() == typeof(GoldAccount).Name)));

            bank.OpenAccount(owner, 10001);
            repoMock.Verify(st => st.Create(It.Is<BankAccountDTO>(acc => acc.Type.ToString() == typeof(PlatinumAccount).Name)));
        }
    }
}