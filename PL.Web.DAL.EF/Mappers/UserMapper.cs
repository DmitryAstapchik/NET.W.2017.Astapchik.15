using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Web.DAL.Interface.DTO;

namespace PL.Web.DAL.EF
{
    public static class UserMapper
    {
        public static User FromDTO(this UserDTO dto)
        {
            return new User { Email = dto.Email, FullName = dto.FullName, Password = dto.Password, PassportID = dto.PassportNumber };
        }

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
