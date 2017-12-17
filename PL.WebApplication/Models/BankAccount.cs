using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.WebApplication.Models
{
    public class BankAccount
    {
        public string IBAN { get; set; }
        public string Owner { get; set; }
        public decimal Balance { get; set; }
        public float BonusPoints { get; set; }
        public string AccountType { get; set; }
    }
}