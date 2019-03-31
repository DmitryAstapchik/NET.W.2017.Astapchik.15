using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using PL.WebApplication.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject.Web.Mvc.Validation;

namespace PL.WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            NinjectModule registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            kernel.Unbind<ModelValidatorProvider>();
            System.Web.Mvc.DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

            //ModelValidatorProviders.Providers.Clear();
            //ModelValidatorProviders.Providers.Add(ModelValidatorProviders.Providers));
        }

        //protected void Application_Error()
        //{
        //    Exception exception = Server.GetLastError();
        //    Server.ClearError();
        //    Response.Redirect("/Home/Error");
        //}
    }
}
