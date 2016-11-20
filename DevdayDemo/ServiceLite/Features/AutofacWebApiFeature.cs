using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using Autofac.Integration.WebApi;
using DevdayDemo.ServiceLite.Autofac;
using DevdayDemo.ServiceLite.Core;

namespace DevdayDemo.ServiceLite.Features
{
    public sealed class AutofacWebApiFeature : IPlugin, IConfigurable, IPostConfigurable
    {
        public AutofacWebApiFeature()
        {
            Assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
        }

        public List<Assembly> Assemblies { get; }

        public void Configure(ConfigurationContext context)
        {
            var builder = ((AutofacServiceCollection)context.ServiceCollection).Builder;
            var assemblies = Assemblies.ToArray();

            // Register your Web API controllers.
            builder.RegisterApiControllers(assemblies);

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(context.AppHost.Get<HttpConfiguration>());
        }

        public void PostConfigure(ConfigurationContext context)
        {
            var container = ((AutofacServiceProvider)context.AppHost.Container).Container;
            var config = context.AppHost.Get<HttpConfiguration>();

            // Set the dependency resolver to be Autofac.
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public void Start(StartContext context)
        {
        }
    }
}