using System;

namespace DAL.Interface
{
    /// <summary>
    /// Represents a bank account with IBAN, owner, balance, bonus points and account type
    /// </summary>
    public class BankAccountDTO
    {
        public BankAccountDTO(string iban, AccountOwnerDTO owner, decimal balance, float bonus, AccountType type)
        {
            IBAN = iban;
            Owner = owner;
            Balance = balance;
            BonusPoints = bonus;
            Type = type;
        }

        public enum AccountType
        {
            StandardAccount,
            GoldAccount,
            PlatinumAccount
        }

        public enum AccountStatus
        {
            Active,
            Inactive
        }

        public string IBAN { get; set; }

        public AccountOwnerDTO Owner { get; set; }

        public decimal Balance { get; set; }

        public float BonusPoints { get; set; }

        public AccountType Type { get; set; }

        public AccountStatus Status {get;set;}
    }
}
