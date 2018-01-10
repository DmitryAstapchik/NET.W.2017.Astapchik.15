using BLL;
using BLL.Interface;
using DAL;
using DAL.DB;
using DAL.DB.Repositories;
using DAL.Fake;
using DAL.Interface;
using DAL.Interface.Interfaces;
using Ninject;

namespace DependencyResolver
{
    public static class ResolverConfig
    {
        public static void ConfigurateResolver(this IKernel kernel)
        {
            kernel.Bind<IBonusPointsCalculator>().To<BonusCalculator>().InSingletonScope();
            kernel.Bind<IIBANGenerator>().To<IBANGenerator>().InSingletonScope();
            kernel.Bind<IAccountsRepository>().To<AccountsRepository>();
            kernel.Bind<IAccountsUnitOfWork>().To<AccountsUnitOfWork>();
            kernel.Bind<IAccountService>().To<Bank>();
        }
    }
}
