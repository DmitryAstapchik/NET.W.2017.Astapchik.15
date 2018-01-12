using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Web.DAL.Interface.DTO;

namespace PL.Web.DAL.EF
{
    /// <summary>
    /// maps EF user to user DTO
    /// </summary>
    internal static class UserMapper
    {
        /// <summary>
        /// gets EF user from user DTO
        /// </summary>
        /// <param name="dto">user DTO</param>
        /// <returns>EF user</returns>
        public static User FromDTO(this UserDTO dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new User { Email = dto.Email, FullName = dto.FullName, Password = dto.Password, PassportID = dto.PassportNumber };
        }

        /// <summary>
        /// projects EF user to user DTO
        /// </summary>
        /// <param name="user">EF user</param>
        /// <returns>user DTO</returns>
        public static UserDTO ToDTO(this User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserDTO { Email = user.Email, FullName = user.FullName, Password = user.Password, PassportNumber = user.PassportID };
        }
    }
}
