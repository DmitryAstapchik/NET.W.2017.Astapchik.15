using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Web.DAL.Interface.DTO;

namespace PL.Web.DAL.Interface.Interfaces
{
    /// <summary>
    /// web app users repository contract
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// add new user to repository
        /// </summary>
        /// <param name="user">user to add</param>
        void Create(UserDTO user);

        /// <summary>
        /// get user by e-mail
        /// </summary>
        /// <param name="email">user's e-mail</param>
        /// <returns>user DTO</returns>
        UserDTO GetByEmail(string email);

        /// <summary>
        /// update user
        /// </summary>
        /// <param name="user">updated user</param>
        void Update(UserDTO user);

        /// <summary>
        /// delete user from repository
        /// </summary>
        /// <param name="user">user to delete</param>
        void Delete(UserDTO user);
    }
}
