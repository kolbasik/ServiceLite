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
            var builder = context.AppHost.Get<ContainerBuilder>();
            var assemblies = Assemblies.ToArray();

            // Register Common MVC Types
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register MVC Filters
            builder.RegisterFilterProvider();

            // Register MVC ModelBindes
            builder.RegisterModelBinderProvider();
            builder.RegisterModelBinders(assemblies);

            // Register MVC Controllers
            builder.RegisterControllers(assemblies);
        }

        public void PostConfigure(ConfigurationContext context)
        {
            var container = context.AppHost.Get<IContainer>();

            // Sets the ASP.NET MVC dependency resolver.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public void Start(StartContext context)
        {
            var container = context.AppHost.Get<IContainer>();

            var app = context.AppHost.Get<IAppBuilder>();

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}