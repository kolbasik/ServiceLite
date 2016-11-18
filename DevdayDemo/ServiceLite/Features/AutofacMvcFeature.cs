using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using DevdayDemo.ServiceLite.Autofac;
using DevdayDemo.ServiceLite.Core;
using Owin;

namespace DevdayDemo.ServiceLite.Features
{
    public sealed class AutofacMvcFeature : Plugin
    {
        public AutofacMvcFeature()
        {
            Assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
        }

        public List<Assembly> Assemblies { get; }

        public override void Configure(IServiceCollection container)
        {
            var builder = ((AutofacServiceCollection) container).Builder;
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

        public override void Register(IAppHost appHost)
        {
            var container = ((AutofacServiceProvider) appHost.Container).Container;

            var app = (IAppBuilder) appHost.Container.GetService(typeof(IAppBuilder));
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            // Sets the ASP.NET MVC dependency resolver.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}