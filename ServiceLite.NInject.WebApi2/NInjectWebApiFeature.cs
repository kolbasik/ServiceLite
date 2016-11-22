using System.Web.Http;
using Ninject;
using Ninject.Web.WebApi;
using ServiceLite.Core;

namespace ServiceLite.NInject.WebApi2
{
    public sealed class NInjectWebApiFeature : IPlugin, IConfigurable
    {
        public void Configure(ConfigurationContext context)
        {
            var kernal = context.AppHost.GetRequired<IKernel>();
            var config = context.AppHost.GetRequired<HttpConfiguration>();

            config.DependencyResolver = new NinjectDependencyResolver(kernal);
        }

        public void Start(StartContext context)
        {
        }
    }
}