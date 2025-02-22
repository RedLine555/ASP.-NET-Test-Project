[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HomeWork3.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(HomeWork3.App_Start.NinjectWebCommon), "Stop")]

namespace HomeWork3.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using HomeWork3Data;
    using System.Configuration;
    using HomeWork3Data.DataModel;
    using HomeWork3Business.Interfaces;
    using HomeWork3Business.Services;
    using HomeWork3Common;
    using HomeWork3Common.Services;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUserRepository>().To<UserRepository>().WithConstructorArgument("cont", new HW3DatabaseEntities());
            //kernel.Bind<IUserRepository>().To<ADOUserRepository>().WithConstructorArgument("connString", ConfigurationManager.ConnectionStrings["HW3DatabaseADO"].ConnectionString);
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IEmailService>().To <EmailService>().WithConstructorArgument("config", ConfigurationManager.AppSettings);
        }        
    }
}
