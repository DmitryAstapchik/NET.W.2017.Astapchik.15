using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IUsersRepository
    {
        void Create(UserDTO user);
        UserDTO GetByEmail(string email);
        void Update(UserDTO user);
        void Delete(UserDTO user);
    }
}
