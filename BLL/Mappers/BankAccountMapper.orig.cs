using BLL.Interface;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class BankAccountMapper
    {
        public static BankAccountDTO ToDTO(BankAccount account)
        {
            return new BankAccountDTO(account.IBAN, account.Owner, account.Balance, account.BonusPoints, account.GetType().FullName);
        }

        public static BankAccount FromDTO(BankAccountDTO dto)
        {
            return (BankAccount)Assembly.GetAssembly(typeof(BankAccount)).GetType(dto.AccountType).GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0].Invoke(new object[] { dto.IBAN, dto.Owner, dto.Balance, dto.BonusPoints });
        }
    }
}
