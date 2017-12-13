using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface;
using DAL.Interface;

namespace BLL
{
    /// <summary>
    /// maps bank account object
    /// </summary>
    public static class BankAccountMapper
    {
        /// <summary>
        /// gets a bank account instance from DTO
        /// </summary>
        /// <param name="dto">bank account DTO</param>
        /// <returns>bank account instance</returns>
        public static BankAccount FromDTO(this AccountDTO dto)
        {
            return (BankAccount)Assembly.GetAssembly(typeof(BankAccount)).GetType(dto.AccountType).GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0].Invoke(new object[] { dto.IBAN, dto.Owner, dto.Balance, dto.BonusPoints });
        }

        /// <summary>
        /// projects a bank account instance to DTO
        /// </summary>
        /// <param name="account">bank account to project</param>
        /// <returns>DTO instance</returns>
        public static AccountDTO ToDTO(this BankAccount account)
        {
            return new AccountDTO(account.IBAN, account.Owner, account.Balance, account.BonusPoints, account.GetType().FullName);
        }
    }
}