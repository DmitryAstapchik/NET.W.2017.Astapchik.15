using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using DAL.Interface.Interfaces;
using ORM.EF;

namespace DAL.DB.Repositories
{
    public class AccountsUnitOfWork : IAccountsUnitOfWork
    {
        private AccountModelContainer context;
        private AccountsRepository accountsRepo;

        public AccountsUnitOfWork()
        {
            context = new AccountModelContainer();
        }

        public IAccountsRepository Accounts => accountsRepo ?? (accountsRepo = new AccountsRepository(context));

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
