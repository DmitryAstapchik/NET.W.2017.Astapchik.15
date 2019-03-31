using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using PL.WebApplication.DAL.EF;
using PL.WebApplication.DAL.Interface.Interfaces;

namespace PL.WebApplication.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IUsersRepository>().To<UsersRepository>();
        }
    }
}