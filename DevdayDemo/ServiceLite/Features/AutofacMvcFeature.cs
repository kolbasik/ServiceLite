using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Owin;
using ServiceLite.Autofac.Core;
using ServiceLite.Core;

namespace DevdayDemo.ServiceLite.Features
{
    public sealed class AutofacMvcFeature : IPlugin, IConfigurable, IPostConfigurable
    {
        public AutofacMvcFeature()
        {
            Assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
        }

        public List<Assembly> Assemblies { get; }

        public void Configure(ConfigurationContext context)
        {
            var builder = ((AutofacServiceCollection)context.ServiceCollection).Builder;
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
            var container = ((AutofacServiceProvider)context.AppHost.Container).Container;

            // Sets the ASP.NET MVC dependency resolver.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public void Start(StartContext context)
        {
            var container = ((AutofacServiceProvider) context.AppHost.Container).Container;

            var app = context.AppHost.Get<IAppBuilder>();

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}