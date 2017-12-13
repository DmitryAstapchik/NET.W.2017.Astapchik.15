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

            // kernel.Bind<IAccountStorage>().To<BinaryFileStorage>().WithConstructorArgument("test.bin");
            // kernel.Bind<IAccountStorage>().To<ListStorage>();
            kernel.Bind<IAccountRepository>().To<DBStorage>();
            kernel.Bind<IIBANGenerator>().To<IBANGenerator>().InSingletonScope();
            kernel.Bind<IAccountService>().To<Bank>();
         
            // kernel.Bind<IApplicationSettings>().To<ApplicationSettings>();
        }
    }
}
