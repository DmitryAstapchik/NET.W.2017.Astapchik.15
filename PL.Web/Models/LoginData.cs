using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.Web.Models
{
    public class LoginData
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       // public string ReturnUrl { get; set; }

        [Display(Name ="Remember me")]
        public bool Remember { get; set; }
    }
}