using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.WebApplication.Models
{
    public class AccountOwner
    {
        [Display(Name = "E-mail address")]
        [UIHint("EmailAddress")]
        public string Email { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Display(Name ="Passport Id")]
        public string PassportId { get; set; }

        public static implicit operator AccountOwner(BLL.Interface.AccountOwner owner)
        {
            return new AccountOwner { PassportId = owner.PassportID, FullName = owner.FullName, Email = owner.Email };
        }
    }
}