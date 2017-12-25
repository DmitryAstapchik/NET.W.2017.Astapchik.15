using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PL.WebApplication.Infrastructure;

namespace PL.WebApplication.Models
{
    // [MinDeposit]
    // [MinWithdrawal]
    public class BankAccount
    {
        //[Required]
        public string IBAN { get; set; }

        [Required]
        [StringLength(30)]
        public string Owner { get; set; } = "default";

        // [Range(0, int.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Range(0, 100)]
        [Display(Name ="Bonus points")]
        public float BonusPoints { get; set; }

        [Display(Name = "Account type")]
        public AccountType Type { get; set; }

        public enum AccountType
        {
           Standard = 0,
            Gold = 1000,
            Platinum = 10000
        }

        public static explicit operator BankAccount(BLL.Interface.BankAccount account)
        {
            return new BankAccount
            {
                IBAN = account.IBAN,
                Owner = account.Owner,
                Balance = account.Balance,
                BonusPoints = account.BonusPoints,
                Type = (AccountType)Enum.Parse(typeof(AccountType), account.GetType().Name.Replace("Account", string.Empty))
            };
        }
    }
}