using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.WebApplication.Infrastructure
{
    public class MinWithdrawalAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ErrorMessage = "Minimum withdrawal amount is 10";
            return value is decimal && (decimal)value >= 10;
        }
    }
}