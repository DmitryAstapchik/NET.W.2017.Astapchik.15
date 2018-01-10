using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
    public interface IAccountsUnitOfWork
    {
        IAccountsRepository Accounts { get; }

        void Save();
    }
}
