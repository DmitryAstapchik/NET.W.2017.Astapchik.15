using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using PL.WebApplication.DAL.Interface;
using PL.WebApplication.DAL.EF;

namespace PL.WebApplication.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IUsersRepository>().To<UsersDBRepository>();
        }
    }
}