using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Interface;
using DAL.Interface;
using Moq;
using NUnit.Framework;

namespace BLL.Tests
{
    [TestFixture]
    public class BankTests
    {
        private static IBonusPointsCalculator calculator = Mock.Of<IBonusPointsCalculator>();
        private static IIBANGenerator generator = Mock.Of<IIBANGenerator>(g => g.GenerateIBAN() == "fake iban");
        private static Mock<AccountDTO> dtoMock = new Mock<AccountDTO>("iban", "owner", 22m, 0, typeof(StandardAccount).FullName);
        private static IAccountRepository storage = Mock.Of<IAccountRepository>(st => st.GetByIban(It.IsAny<string>()) == dtoMock.Object);
        private static Bank bank = new Bank(storage, generator, calculator);
        private static Mock<IAccountRepository> storageMock = Mock.Get(storage);

        [Test]
        public void CloseAccountTest()
        {
            Assert.Throws<ArgumentException>(() => bank.CloseAccount(string.Empty));
            Assert.Throws<ArgumentException>(() => bank.CloseAccount(null));
            Assert.Throws<ArgumentException>(() => bank.CloseAccount(" "));

            bank.CloseAccount("124");
            storageMock.Verify(st => st.Delete(It.Is<AccountDTO>(dto => dto.IBAN == "124")));
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
            storageMock.Verify(st => st.Update(It.Is<AccountDTO>(dto => dto.Balance == 122)));
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
            storageMock.Verify(st => st.Update(It.Is<AccountDTO>(dto => dto.Balance == 0)));
        }

        [Test]
        public void OpenAccountTest()
        {
            Assert.Throws<ArgumentException>(() => bank.OpenAccount(string.Empty, 100));
            Assert.Throws<ArgumentException>(() => bank.OpenAccount(null, 100));
            Assert.Throws<ArgumentException>(() => bank.OpenAccount(" ", 100));
            Assert.Throws<ArgumentException>(() => bank.OpenAccount("holder", 49));
            Assert.DoesNotThrow(() => bank.OpenAccount("holder", 50));

            bank.OpenAccount("holder", 1000);
            storageMock.Verify(st => st.Create(It.Is<AccountDTO>(acc => acc.Type == typeof(StandardAccount).FullName)));

            bank.OpenAccount("holder", 1001);
            storageMock.Verify(st => st.Create(It.Is<AccountDTO>(acc => acc.Type == typeof(GoldAccount).FullName)));

            bank.OpenAccount("holder", 10000);
            storageMock.Verify(st => st.Create(It.Is<AccountDTO>(acc => acc.Type == typeof(GoldAccount).FullName)));

            bank.OpenAccount("holder", 10001);
            storageMock.Verify(st => st.Create(It.Is<AccountDTO>(acc => acc.Type == typeof(PlatinumAccount).FullName)));
        }
    }
}