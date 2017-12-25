using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PL.WebApplication.Models
{
    public class RegistrationData
    {
        [Required]
        [DataType(DataType.EmailAddress)]
       // [Display(Name = "e-mail address")]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }


        [Required]
        [DataType(DataType.Password)]
       // [Display(Name ="password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmedPassword { get; set; }

    }
}