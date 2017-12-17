using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;

namespace DAL.Fake
{
    public class ListStorage : IAccountRepository
    {
        private List<AccountDTO> storage = new List<AccountDTO>
        {
            new AccountDTO("111", "owner1", 100, 23.34f, AccountDTO.AccountType.GoldAccount),
            new AccountDTO("222", "owner2", 200, 0.55f, AccountDTO.AccountType.PlatinumAccount),
               new AccountDTO("123", "owner1", 300, 34.34f, AccountDTO.AccountType.StandardAccount)
        };

        public void Create(AccountDTO account)
        {
            if (storage.Contains(account))
            {
                throw new ApplicationException($"list already contains an account with IBAN {account.IBAN}");
            }

            storage.Add(account);
        }

        public AccountDTO GetByIban(string iban)
        {
            return storage.Find(acc => acc.IBAN == iban) ?? throw new ApplicationException($"account with IBAN {iban} was not found");
        }

        public void Delete(AccountDTO account)
        {
            if (!storage.Remove(account))
            {
                throw new ApplicationException($"account with IBAN {account.IBAN} was not removed");
            }
        }

        public void Update(AccountDTO account)
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

        public IEnumerable<AccountDTO> GetAllAccounts()
        {
            return this.storage;
        }
    }
}
