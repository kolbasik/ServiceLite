using Autofac;
using ServiceLite.Core;

namespace ServiceLite.Autofac.Core
{
    public sealed class AutofacFeature : IPlugin, IPreConfigurable, IPostConfigurable
    {
        public void PreConfigure(ConfigurationContext context)
        {
            var builder = ((AutofacServiceCollection)context.ServiceCollection).Builder;

            context.AppHost.Set<IAppHost, ContainerBuilder>(builder);
        }

        public void PostConfigure(ConfigurationContext context)
        {
            var container = ((AutofacServiceProvider)context.AppHost.Container).Container;

            context.AppHost.Set<IAppHost, IContainer>(container);
        }

        public void Start(StartContext context)
        {
        }
    }
}