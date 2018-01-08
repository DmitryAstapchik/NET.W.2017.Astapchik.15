using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using ORM.EF;

namespace DAL.DB
{
    public class AccountsDBRepository : IAccountsRepository
    {
        AccountModelContainer context;
        public AccountsDBRepository(AccountModelContainer context)
        {
            this.context = context;
        }

        public void Create(BankAccountDTO account)
        {
           // using (var context = new AccountModelContainer())
           // {
                context.BankAccountSet.Add(account.FromDTO());
                //var owner = context.OwnerSet.Find(account.Owner.PassportID);
                //if (owner == null)
                //{
                //    context.OwnerSet.Add(account.Owner.FromDTO());
                //}
                //else
                //{
                //    owner.BankAccounts.Add(account.FromDTO());
                //}
            //    context.SaveChanges();
           // }
        }

        public void Delete(BankAccountDTO account)
        {
           // using (var context = new AccountModelContainer())
          //  {
                context.BankAccountSet.Remove(context.BankAccountSet.Find(account.IBAN));
            //    context.SaveChanges();
           // }
        }

        public IEnumerable<BankAccountDTO> GetAllAccounts()
        {
          //  using (var context = new AccountModelContainer())
          //  {
                return context.BankAccountSet.ToArray().Select(a => a.ToDTO());
          //  }
        }

        public BankAccountDTO GetByIban(string iban)
        {
          //  using (var context = new AccountModelContainer())
          //  {
                var account = context.BankAccountSet.Find(iban);
                return account.ToDTO();
          //  }
        }

        public IEnumerable<BankAccountDTO> GetByOwner(AccountOwnerDTO owner)
        {
         //   using (var context = new AccountModelContainer())
         //   {
                return context.AccountOwnerSet.Find(owner.PassportID)?.BankAccounts.Select(a => a.ToDTO());
           // }
        }

        public void Update(BankAccountDTO account)
        {
            //using (var context = new AccountModelContainer())
            //{
            //    var toUpdate = context.BankAccountSet.Find(account.IBAN);
            //    toUpdate.AccountType = account.Type.ToString();
            //    toUpdate.Balance = account.Balance;
            //    toUpdate.BonusPoints = account.BonusPoints;
            //    toUpdate.Owner.FullName = account.Owner.FullName;
            //    toUpdate.Owner.PassportID = account.Owner.PassportID;
            //    toUpdate.Owner.Email = account.Owner.Email;
            //    toUpdate.Status = account.Status.ToString();
            //    context.SaveChanges();
            //}

            //context.Entry (account.FromDTO()).State = System.Data.Entity.EntityState.Modified;

            var acc = context.BankAccountSet.Find(account.IBAN);
            acc.AccountType = account.Type.ToString();
            acc.Balance = account.Balance;
            acc.BonusPoints = account.BonusPoints;
            acc.Owner.FullName = account.Owner.FullName;
            acc.Owner.PassportID = account.Owner.PassportID;
            acc.Owner.Email = account.Owner.Email;
            acc.Status = account.Status.ToString();
        }
    }
}
