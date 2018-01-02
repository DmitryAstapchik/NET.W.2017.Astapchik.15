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
        public static BankAccount FromDTO(this BankAccountDTO dto)
        {
            return (BankAccount)Activator.CreateInstance(
                Assembly.GetAssembly(typeof(BankAccount)).GetType("BLL.Interface." + dto.Type.ToString()),
                new object[] { dto.IBAN, new AccountOwner(dto.Owner.PassportID, dto.Owner.FullName, dto.Owner.Email), dto.Balance, dto.BonusPoints, (BankAccount.BankAccountStatus)(int)dto.Status });
        }

        /// <summary>
        /// projects a bank account instance to DTO
        /// </summary>
        /// <param name="account">bank account to project</param>
        /// <returns>DTO instance</returns>
        public static BankAccountDTO ToDTO(this BankAccount account)
        {
            return new BankAccountDTO(
                account.IBAN,
                new AccountOwnerDTO { PassportID = account.Owner.PassportID, FullName = account.Owner.FullName, Email = account.Owner.Email },
                account.Balance,
                account.BonusPoints,
                (BankAccountDTO.AccountType)Enum.Parse(typeof(BankAccountDTO.AccountType), account.GetType().Name))
            { Status = (BankAccountDTO.AccountStatus)(int)account.Status };
        }
    }
}