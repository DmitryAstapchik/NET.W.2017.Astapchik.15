using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;

namespace DAL.Fake
{
    public class ListRepository : IAccountsRepository
    {
        private List<BankAccountDTO> storage = new List<BankAccountDTO>
        {
            new BankAccountDTO( "111", new AccountOwnerDTO("pass1", "owner1", "email1" ), 100,23.34f,BankAccountDTO.AccountType.GoldAccount ),
            new BankAccountDTO("222", new AccountOwnerDTO( "pass2",  "owner2",  "email2" ), 200, 0.55f, BankAccountDTO.AccountType.PlatinumAccount),
            new BankAccountDTO("123", new AccountOwnerDTO( "pass1", "owner1",  "email1" ), 300, 34.34f, BankAccountDTO.AccountType.StandardAccount)
        };

        public void Create(BankAccountDTO account)
        {
            if (storage.Contains(account))
            {
                throw new ApplicationException($"list already contains an account with IBAN {account.IBAN}");
            }

            storage.Add(account);
        }

        public BankAccountDTO GetByIban(string iban)
        {
            return storage.Find(acc => acc.IBAN == iban) ?? throw new ApplicationException($"account with IBAN {iban} was not found");
        }

        public void Delete(BankAccountDTO account)
        {
            if (!storage.Remove(account))
            {
                throw new ApplicationException($"account with IBAN {account.IBAN} was not removed");
            }
        }

        public void Update(BankAccountDTO account)
        {
            int accountIndex = storage.FindIndex(acc => acc.IBAN == account.IBAN);
            if (accountIndex >= 0)
            {
                storage.RemoveAt(accountIndex);
                storage.Insert(accountIndex, account);
            }
            else
            {
                throw new ApplicationException($"account with IBAN { account.IBAN } was not found");
            }
        }

        public IEnumerable<BankAccountDTO> GetAllAccounts()
        {
            return this.storage;
        }

        public IEnumerable<BankAccountDTO> GetByOwner(AccountOwnerDTO owner)
        {
            throw new NotImplementedException();
        }
    }
}