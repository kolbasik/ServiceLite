using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using ServiceLite.Core;

namespace ServiceLite.Autofac.WebApi2
{
    public sealed class AutofacWebApiFeature : IPlugin, IConfigurable, IPostConfigurable
    {
        public AutofacWebApiFeature()
        {
            Assemblies = new List<Assembly> { Assembly.GetCallingAssembly() };
        }

        public List<Assembly> Assemblies { get; }

        public void Configure(ConfigurationContext context)
        {
            var containerBuilder = context.AppHost.GetRequired<ContainerBuilder>();
            var assemblies = Assemblies.ToArray();

            // Register your Web API controllers.
            containerBuilder.RegisterApiControllers(assemblies);

            // OPTIONAL: Register the Autofac filter provider.
            containerBuilder.RegisterWebApiFilterProvider(context.AppHost.GetRequired<HttpConfiguration>());
        }

        public void PostConfigure(ConfigurationContext context)
        {
            var container = context.AppHost.GetRequired<ILifetimeScope>();
            var config = context.AppHost.GetRequired<HttpConfiguration>();

            // Set the dependency resolver to be Autofac.
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public void Start(StartContext context)
        {
        }
    }
}