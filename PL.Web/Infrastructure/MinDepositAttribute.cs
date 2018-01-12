using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.Web.Infrastructure
{
    public class MinDepositAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ErrorMessage = "Minimum deposit amount is 50";
            return value is decimal && (decimal)value >= 50;
        }
    }
}