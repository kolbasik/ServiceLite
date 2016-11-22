using Ninject.Web.Common;
using ServiceLite.Core;

namespace ServiceLite.NInject.Core
{
    public sealed class NInjectFeature : IPlugin, IPreConfigurable
    {
        public void PreConfigure(ConfigurationContext context)
        {
            var kernel = ((NInjectServiceCollection)context.Services).Kernel;

            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialize(() => kernel);
            context.AppHost.CancellationToken.Register(() => bootstrapper.ShutDown());

            context.AppHost.Set(kernel);
        }

        public void Start(StartContext context)
        {
        }
    }
}