using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.Web.Models
{
    public class NewAccount : IValidatableObject
    {
        [Display(Name ="Start balance")]
        [DataType(DataType.Currency)]
        [Range(0, 1_000_000)]
        public decimal StartBalance { get; set; }

        [Display(Name = "Account type")]
        [UIHint("Enum")]
        public  AccountType Type { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type == AccountType.Standard && StartBalance < (int)AccountType.Standard)
            {
                yield return new ValidationResult($"minimal start balance of a standard account is {(int)AccountType.Standard}");
            }

            if (Type == AccountType.Gold && StartBalance < (int)AccountType.Gold)
            {
                yield return new ValidationResult($"minimal start balance of a gold account is {(int)AccountType.Gold}");
            }

            if (Type == AccountType.Platinum && StartBalance < (int)AccountType.Platinum)
            {
                yield return new ValidationResult($"minimal start balance of a platinum account is {(int)AccountType.Platinum}");
            }
        }

        public enum AccountType
        {
            Standard = 0,
            Gold = 1000,
            Platinum = 10000
        }
    }
}