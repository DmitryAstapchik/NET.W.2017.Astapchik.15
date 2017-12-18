using System;

namespace DAL.Interface
{
    /// <summary>
    /// Represents a bank account with IBAN, owner, balance, bonus points and account type
    /// </summary>
    public class AccountDTO
    {
        public AccountDTO(string iban, string owner, decimal balance, float bonus, AccountType type)
        {
            IBAN = iban;
            Owner = owner;
            Balance = balance;
            BonusPoints = bonus;
            Type = type;
        }

        public enum AccountType { StandardAccount, GoldAccount, PlatinumAccount };

        public string IBAN { get; private set; }

        public string Owner { get; private set; }

        public decimal Balance { get; private set; }

        public float BonusPoints { get; private set; }

        public AccountType Type { get; private set; }
    }
}
