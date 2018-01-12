using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Interface;
using DAL.Interface;
using NUnit.Framework;

namespace BLL.Tests
{
    [TestFixture]
    public class BankAccountMapperTests
    {
        private static IEnumerable<BankAccountDTO> FromDTOTestCases
        {
            get
            {
                yield return new BankAccountDTO("123", new AccountOwnerDTO("123", "owner1", "owner1mail"), 120m, 2.33f, BankAccountDTO.AccountType.StandardAccount);
                yield return new BankAccountDTO("456", new AccountOwnerDTO("456", "owner2", "owner2mail"), 5000m, 5.44f, BankAccountDTO.AccountType.GoldAccount);
                yield return new BankAccountDTO("789", new AccountOwnerDTO("789", "owner3", "owner3mail"), 10001m, 3.11f, BankAccountDTO.AccountType.PlatinumAccount);
            }
        }

        private static IEnumerable<BankAccount> ToDTOTestCases
        {
            get
            {
                yield return new StandardAccount("123", new AccountOwner("123", "owner1", "owner1mail"), 120m, 2.33f);
                yield return new GoldAccount("456", new AccountOwner("456", "owner2", "owner2mail"), 50m, 5.44f);
                yield return new PlatinumAccount("789", new AccountOwner("789", "owner3", "owner3mail"), 100m, 3.11f);
            }
        }

        [TestCaseSource("FromDTOTestCases")]
        public void FromDTOTest(BankAccountDTO dto)
        {
            var account = dto.FromDTO();
            Assert.That(account.IBAN == dto.IBAN &&
                account.Owner.Email == dto.Owner.Email &&
                account.Owner.FullName == dto.Owner.FullName &&
                account.Owner.PassportID == dto.Owner.PassportID &&
                account.Balance == dto.Balance &&
                account.BonusPoints == dto.BonusPoints &&
                account.GetType().Name == dto.Type.ToString());
        }

        [TestCaseSource("ToDTOTestCases")]
        public void ToDTOTest(BankAccount account)
        {
            var dto = account.ToDTO();
            Assert.That(dto.IBAN == account.IBAN &&
                dto.Owner.Email == account.Owner.Email &&
                dto.Owner.FullName == account.Owner.FullName &&
                dto.Owner.PassportID == account.Owner.PassportID &&
                 dto.Balance == account.Balance &&
                 dto.BonusPoints == account.BonusPoints &&
                 dto.Type.ToString() == account.GetType().Name);
        }
    }
}