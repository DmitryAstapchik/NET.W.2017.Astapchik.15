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
    public class AccountsEFUnitOfWork : IAccountsUnitOfWork
    {
        private AccountModelContainer context;
        private AccountsDBRepository accountsRepo;

        public AccountsEFUnitOfWork()
        {
            context = new AccountModelContainer();
        }

        public IAccountsRepository Accounts => accountsRepo ?? (accountsRepo = new AccountsDBRepository(context));

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
