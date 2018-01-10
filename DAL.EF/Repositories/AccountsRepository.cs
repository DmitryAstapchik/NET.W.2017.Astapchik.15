using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using ORM.EF;

namespace DAL.DB
{
    public class AccountsRepository : IAccountsRepository
    {
        private AccountModelContainer context;

        public AccountsRepository(AccountModelContainer context)
        {
            this.context = context;
        }

        public void Create(BankAccountDTO account)
        {
            if (context.AccountOwnerSet.Find(account.Owner.PassportID) == null)
            {
                context.AccountOwnerSet.Add(account.Owner.FromDTO());
            }

            context.BankAccountSet.Add(account.FromDTO());
        }

        public void Delete(BankAccountDTO account)
        {
            context.BankAccountSet.Remove(context.BankAccountSet.Find(account.IBAN));
        }

        public IEnumerable<BankAccountDTO> GetAllAccounts()
        {
            return context.BankAccountSet.ToArray().Select(a => a.ToDTO());
        }

        public BankAccountDTO GetByIban(string iban)
        {
            var account = context.BankAccountSet.Find(iban);
            return account.ToDTO();
        }

        public IEnumerable<BankAccountDTO> GetByOwner(AccountOwnerDTO owner)
        {
            return context.AccountOwnerSet.Find(owner.PassportID)?.BankAccounts.Select(a => a.ToDTO());
        }

        public void Update(BankAccountDTO account)
        {
            var acc = context.BankAccountSet.Find(account.IBAN);
            acc.AccountType = account.Type.ToString();
            acc.Balance = account.Balance;
            acc.BonusPoints = account.BonusPoints;
            acc.Status = account.Status.ToString();
        }
    }
}
