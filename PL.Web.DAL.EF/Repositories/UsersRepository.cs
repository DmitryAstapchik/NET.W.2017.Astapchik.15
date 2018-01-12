using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Web.DAL.Interface.DTO;
using PL.Web.DAL.Interface.Interfaces;

namespace PL.Web.DAL.EF
{
    /// <summary>
    /// users DB repository
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        /// <summary>
        /// adds new user to repo
        /// </summary>
        /// <param name="user">user to add</param>
        public void Create(UserDTO user)
        {
            using (var context = new UsersContainer())
            {
                context.UserSet.Add(user.FromDTO());
                context.SaveChanges();
            }
        }

        /// <summary>
        /// delete user from repo
        /// </summary>
        /// <param name="user">user to delete</param>
        public void Delete(UserDTO user)
        {
            using (var context = new UsersContainer())
            {
                context.UserSet.Remove(context.UserSet.Find(user.Email));
                context.SaveChanges();
            }
        }

        /// <summary>
        /// get user by his e-mail
        /// </summary>
        /// <param name="email">user's e-mail</param>
        /// <returns>user DTO</returns>
        public UserDTO GetByEmail(string email)
        {
            using (var context = new UsersContainer())
            {
                return context.UserSet.Find(email).ToDTO();
            }
        }

        /// <summary>
        /// updates user in repo
        /// </summary>
        /// <param name="user">updated user</param>
        public void Update(UserDTO user)
        {
            using (var context = new UsersContainer())
            {
                var update = context.UserSet.Find(user.Email);
                update.FullName = user.FullName;
                update.Password = user.Password;
                context.SaveChanges();
            }
        }
    }
}
