using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using ORM.EF;

namespace DAL.DB
{
    internal static class AccountMapper
    {
        public static Account FromDTO(this BankAccountDTO dto)
        {
            return new Account
            {
                AccountType = dto.Type.ToString(),
                Balance = dto.Balance,
                BonusPoints = dto.BonusPoints,
                IBAN = dto.IBAN,
                Owner = dto.Owner
            };
        }

        public static BankAccountDTO ToDTO(this Account account)
        {
            if (account == null)
            {
                return null;
            }

            return new BankAccountDTO(
                account.IBAN,
                account.Owner,
                account.Balance,
                account.BonusPoints,
                (BankAccountDTO.AccountType)Enum.Parse(typeof(BankAccountDTO.AccountType), account.AccountType));
        }
    }
}
