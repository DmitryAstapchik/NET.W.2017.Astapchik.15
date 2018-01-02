using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.WebApplication.Models
{
    public class NewAccount
    {
        [Display(Name ="Start balance")]
        public decimal StartBalance { get; set; }

        [Display(Name = "Account type")]
        public AccountType Type { get; set; }

        public enum AccountType
        {
            Standard = 0,
            Gold = 1000,
            Platinum = 10000
        }
    }
}