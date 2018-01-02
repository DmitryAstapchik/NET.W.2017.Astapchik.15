using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface;

namespace PL.WebApplication.DAL.Interface
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PassportID { get; set; }

        public static implicit operator AccountOwner(UserDTO user)
        {
            return new AccountOwner(user.PassportID, user.FullName, user.Email);
        }
    }
}
