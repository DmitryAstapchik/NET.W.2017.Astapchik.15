using BLL;
using BLL.Interface;
using DAL;
using DAL.DB;
using DAL.Fake;
using DAL.Interface;
using Ninject;

namespace DependencyResolver
{
    public static class ResolverConfig
    {
        public static void ConfigurateResolver(this IKernel kernel)
        {
            kernel.Bind<IBonusPointsCalculator>().To<BonusCalculator>();
            kernel.Bind<IIBANGenerator>().To<IBANGenerator>().InSingletonScope();

            // kernel.Bind<IAccountStorage>().To<BinaryFileStorage>().WithConstructorArgument("test.bin");
            //kernel.Bind<IAccountRepository>().To<ListRepository>();
            kernel.Bind<IAccountRepository>().To<AccountsDBRepository>();
            //kernel.Bind<IUsersRepository>().To<UsersDBRepository>();
            kernel.Bind<IAccountService>().To<Bank>();
         
            // kernel.Bind<IApplicationSettings>().To<ApplicationSettings>();
        }
    }
}
