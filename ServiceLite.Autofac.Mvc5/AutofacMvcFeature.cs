using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Owin;
using ServiceLite.Core;

namespace ServiceLite.Autofac.Mvc5
{
    public sealed class AutofacMvcFeature : IPlugin, IConfigurable, IPostConfigurable
    {
        public AutofacMvcFeature()
        {
            Assemblies = new List<Assembly> { Assembly.GetCallingAssembly() };
        }

        public List<Assembly> Assemblies { get; }

        public void Configure(ConfigurationContext context)
        {
            var containerBuilder = context.AppHost.GetRequired<ContainerBuilder>();
            var assemblies = Assemblies.ToArray();

            // Register Common MVC Types
            containerBuilder.RegisterModule<AutofacWebTypesModule>();

            // Register MVC Filters
            containerBuilder.RegisterFilterProvider();

            // Register MVC ModelBindes
            containerBuilder.RegisterModelBinderProvider();
            containerBuilder.RegisterModelBinders(assemblies);

            // Register MVC Controllers
            containerBuilder.RegisterControllers(assemblies);
        }

        public void PostConfigure(ConfigurationContext context)
        {
            var container = context.AppHost.GetRequired<ILifetimeScope>();

            // Sets the ASP.NET MVC dependency resolver.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public void Start(StartContext context)
        {
            var container = context.AppHost.GetRequired<ILifetimeScope>();

            var app = context.AppHost.GetRequired<IAppBuilder>();

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}