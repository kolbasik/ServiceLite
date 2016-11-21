using Autofac;
using ServiceLite.Core;

namespace ServiceLite.Autofac.Core
{
    public sealed class AutofacFeature : IPlugin, IPreConfigurable, IPostConfigurable
    {
        public void PreConfigure(ConfigurationContext context)
        {
            var builder = ((AutofacServiceCollection)context.Services).Builder;

            context.AppHost.Set(builder);
        }

        public void PostConfigure(ConfigurationContext context)
        {
            var container = ((AutofacServiceProvider)context.AppHost.Container).Container;

            context.AppHost.Set(container);
        }

        public void Start(StartContext context)
        {
        }
    }
}