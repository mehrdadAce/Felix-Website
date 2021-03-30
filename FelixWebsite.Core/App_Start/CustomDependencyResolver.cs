using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Ninject.Syntax;
using Ninject.Web.WebApi;

namespace FelixWebsite.Core
{
    public class CustomDependencyResolver : NinjectDependencyResolver, IHttpControllerActivator
    {
        private readonly DefaultHttpControllerActivator defaultHttpControllerActivator;

        public CustomDependencyResolver(IResolutionRoot root) : base(root)
        {
            defaultHttpControllerActivator = new DefaultHttpControllerActivator();
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IHttpController instance =
                IsUmbracoController(controllerType)
                    ? Activator.CreateInstance(controllerType) as IHttpController
                    : defaultHttpControllerActivator.Create(request, controllerDescriptor, controllerType);

            return instance;
        }
        private bool IsUmbracoController(Type controllerType)
        {
            return controllerType.Namespace != null
                   && controllerType.Namespace.StartsWith("umbraco", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
