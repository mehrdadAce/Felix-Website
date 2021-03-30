using FelixWebsite.Bll.Services;
using FelixWebsite.Bll.Services.Interfaces;
using FelixWebsite.Dal.UnitOfWork;
using Ninject;
using Ninject.Extensions.Conventions;

namespace FelixWebsite.Bll.Helpers.Configuration
{
    public class NinjectConfig
    {
        public static void Configure(IKernel kernel)
        {
            kernel.Bind(x =>
            {
                x.FromThisAssembly().SelectAllClasses().BindAllInterfaces();
                x.FromAssemblyContaining<UnitOfWork>().SelectAllClasses().BindAllInterfaces();
                
            });
        }
    }
}
