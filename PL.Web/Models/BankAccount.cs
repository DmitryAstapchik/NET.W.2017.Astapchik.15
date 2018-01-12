using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PL.Web.Infrastructure;

namespace PL.Web.Models
{
    // [MinDeposit]
    // [MinWithdrawal]
    public class BankAccount
    {
        [Required]
        public string IBAN { get; set; }

        [Required]
        public string Owner { get; set; }

        // [Range(0, int.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Range(0, 100)]
        [Display(Name ="Bonus points")]
        public float BonusPoints { get; set; }

        [Required]
        [Display(Name ="Account type")]
        public string AccountType { get; set; }

        [Required]
        [Display(Name = "Account status")]
        public BLL.Interface.BankAccount.BankAccountStatus Status { get; set; }

        public static implicit operator BankAccount(BLL.Interface.BankAccount account)
        {
            return new BankAccount
            {
                IBAN = account.IBAN,
                Owner = account.Owner.FullName,
                Balance = account.Balance,
                BonusPoints = account.BonusPoints,
                AccountType = account.GetType().Name.Replace("Account", string.Empty),
                Status = account.Status //(AccountType)Enum.Parse(typeof(AccountType), account.GetType().Name.Replace("Account", string.Empty))
            };
        }
    }
}