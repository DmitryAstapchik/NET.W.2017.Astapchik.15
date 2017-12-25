using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using ORM.EF;

namespace DAL.DB
{
    public class AccountsDBRepository : IAccountRepository
    {
        public void Create(BankAccountDTO account)
        {
            using (var context = new AccountModelContainer())
            {
                context.AccountSet.Add(account.FromDTO());
                context.SaveChanges();
            }
        }

        public void Delete(BankAccountDTO account)
        {
            using (var context = new AccountModelContainer())
            {
                context.AccountSet.Remove(context.AccountSet.Find(account.IBAN));
                context.SaveChanges();
            }
        }

        public IEnumerable<BankAccountDTO> GetAllAccounts()
        {
            using (var context = new AccountModelContainer())
            {
                return context.AccountSet.ToArray().Select(a => a.ToDTO());
            }
        }

        public BankAccountDTO GetByIban(string iban)
        {
            using (var context = new AccountModelContainer())
            {
                var account = context.AccountSet.Find(iban);
                return account.ToDTO();
            }
        }

        public IEnumerable<BankAccountDTO> GetUserAccounts(string email)
        {
            using (var context = new AccountModelContainer())
            {
                return context.UserSet.Find(email).Accounts.Select(a => a.ToDTO());
            }
        }

        public void Update(BankAccountDTO account)
        {
            using (var context = new AccountModelContainer())
            {
                var toUpdate = context.AccountSet.Find(account.IBAN);
                toUpdate.AccountType = account.Type.ToString();
                toUpdate.Balance = account.Balance;
                toUpdate.BonusPoints = account.BonusPoints;
                toUpdate.Owner = account.Owner;
                context.SaveChanges();
            }
        }
    }
}
