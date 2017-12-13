using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using ORM.EF;

namespace DAL.DB
{
    internal static class ORMMapper
    {
        public static Account FromDTO(this AccountDTO dto)
        {
            return new Account
            {
                AccountType = dto.AccountType,
                Balance = dto.Balance,
                BonusPoints = dto.BonusPoints,
                IBAN = dto.IBAN,
                Owner = dto.Owner
            };
        }

        public static AccountDTO ToDTO(this Account account)
        {
            return new AccountDTO(
                account.IBAN,
                account.Owner,
                account.Balance,
                account.BonusPoints,
                account.AccountType);
        }
    }
}
