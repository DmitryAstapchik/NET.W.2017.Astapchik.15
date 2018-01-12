using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface;
using Moq;
using NUnit.Framework;

namespace BLL.Tests
{
    [TestFixture]
    public class BankAccountTests
    {
        [Test]
        public void StandardAccountTest()
        {
            var owner = new AccountOwner("pid", "name", "email");

            Assert.Throws<ArgumentException>(() => new StandardAccount(string.Empty, owner, 22, 22));
            Assert.Throws<ArgumentException>(() => new StandardAccount(null, owner, 22, 22));
            Assert.Throws<ArgumentException>(() => new StandardAccount(" ", owner, 22, 22));

            Assert.Throws<ArgumentNullException>(() => new StandardAccount("iban", null, 22, 22));

            Assert.Throws<ArgumentOutOfRangeException>(() => new StandardAccount("iban", owner, 22, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new StandardAccount("iban", owner, 22, 101));

            var account = new StandardAccount("iban", owner, 22, 22);
            account.BonusPoints -= 55;
            Assert.AreEqual(0, account.BonusPoints);
            account.BonusPoints += 155;
            Assert.AreEqual(100, account.BonusPoints);
            account.BonusPoints -= 55;
            Assert.AreEqual(45, account.BonusPoints);
        }
    }
}