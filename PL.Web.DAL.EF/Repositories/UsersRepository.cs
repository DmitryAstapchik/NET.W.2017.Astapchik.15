using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.WebApplication.DAL.Interface;

namespace PL.WebApplication.DAL.EF
{
    public class UsersDBRepository : IUsersRepository
    {
        public void Create(UserDTO user)
        {
            using (var context = new UsersContainer())
            {
                context.UserSet.Add(user.FromDTO());
                context.SaveChanges();
            }
        }

        public void Delete(UserDTO user)
        {
            using (var context = new UsersContainer())
            {
                context.UserSet.Remove(context.UserSet.Find(user.Email));
                context.SaveChanges();
            }
        }

        public UserDTO GetByEmail(string email)
        {
            using (var context = new UsersContainer())
            {
                return context.UserSet.Find(email).ToDTO();
            }
        }

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
