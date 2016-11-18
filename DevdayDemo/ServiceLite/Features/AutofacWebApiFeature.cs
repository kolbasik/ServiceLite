using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using Autofac.Integration.WebApi;
using DevdayDemo.ServiceLite.Autofac;
using DevdayDemo.ServiceLite.Core;
using Owin;

namespace DevdayDemo.ServiceLite.Features
{
    public sealed class AutofacWebApiFeature : Plugin
    {
        public AutofacWebApiFeature()
        {
            Assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
        }

        public List<Assembly> Assemblies { get; }

        public override void Configure(IAppHost appHost, IServiceCollection container)
        {
            var builder = ((AutofacServiceCollection)container).Builder;
            var assemblies = Assemblies.ToArray();

            // Register your Web API controllers.
            builder.RegisterApiControllers(assemblies);

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(appHost.Get<HttpConfiguration>());
        }


        public override void Register(IAppHost appHost)
        {
            var container = ((AutofacServiceProvider)appHost.Container).Container;
            var config = appHost.Get<HttpConfiguration>();

            // Set the dependency resolver to be Autofac.
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}