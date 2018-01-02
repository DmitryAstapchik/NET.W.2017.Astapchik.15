using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PL.WebApplication.Models
{
    public class RegistrationData
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Passport ID")]
        public string PassportID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        [Display(Name = "Full name")]
        public string FullName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmedPassword { get; set; }

    }
}