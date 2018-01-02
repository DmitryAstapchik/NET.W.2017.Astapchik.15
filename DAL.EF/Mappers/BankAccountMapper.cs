using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using ORM.EF;

namespace DAL.DB
{
    internal static class BankAccountMapper
    {
        public static BankAccount FromDTO(this BankAccountDTO dto)
        {
            return new BankAccount
            {
                AccountType = dto.Type.ToString(),
                Balance = dto.Balance,
                BonusPoints = dto.BonusPoints,
                IBAN = dto.IBAN,
                OwnerPID = dto.Owner.PassportID,
                //Owner = new AccountOwner { FullName = dto.Owner.FullName, PassportID = dto.Owner.PassportID, Email = dto.Owner.Email },
                Status = dto.Status.ToString()
            };
        }

        public static BankAccountDTO ToDTO(this BankAccount account)
        {
            if (account == null)
            {
                return null;
            }

            return new BankAccountDTO(
                account.IBAN,
                new AccountOwnerDTO { PassportID = account.Owner.PassportID, FullName = account.Owner.FullName, Email = account.Owner.Email },
                account.Balance,
                account.BonusPoints,
                (BankAccountDTO.AccountType)Enum.Parse(typeof(BankAccountDTO.AccountType), account.AccountType))
            { Status = (BankAccountDTO.AccountStatus)Enum.Parse(typeof(BankAccountDTO.AccountStatus), account.Status, true) };
        }
    }
}
