using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using ORM.EF;

namespace DAL.DB
{
    public class DBStorage : IAccountRepository
    {
        public void Create(AccountDTO account)
        {
            using (var context = new AccountModelContainer())
            {
                context.AccountSet.Add(account.FromDTO());
                context.SaveChanges();
            }
        }

        public void Delete(AccountDTO account)
        {
            using (var context = new AccountModelContainer())
            {
                context.AccountSet.Remove(context.AccountSet.Find(account.IBAN));
                context.SaveChanges();
            }
        }

        public IEnumerable<AccountDTO> GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public AccountDTO GetByIban(string iban)
        {
            using (var context = new AccountModelContainer())
            {
                var account = context.AccountSet.Find(iban);
                return account.ToDTO();
            }
        }

        public void Update(AccountDTO account)
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
