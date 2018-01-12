using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface;

namespace PL.Web.DAL.Interface.DTO
{
    /// <summary>
    /// web app user DTO
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// user's e-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// user's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// user's full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// user's passport number
        /// </summary>
        public string PassportNumber { get; set; }

        /// <summary>
        /// converts web app user to account owner
        /// </summary>
        /// <param name="user">web app user</param>
        public static implicit operator AccountOwner(UserDTO user)
        {
            return new AccountOwner(user.PassportNumber, user.FullName, user.Email);
        }
    }
}
