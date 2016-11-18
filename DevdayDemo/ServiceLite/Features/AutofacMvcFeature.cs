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
        public override void Configure(IServiceCollection container)
        {
            var builder = ((AutofacServiceCollection) container).Builder;

            // Register Common MVC Types
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register MVC Filters
            builder.RegisterFilterProvider();

            // Register MVC Controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
        }

        public override void Register(IAppHost appHost)
        {
            var autofac = ((AutofacServiceProvider) appHost.Container).Container;

            var app = (IAppBuilder) appHost.Container.GetService(typeof(IAppBuilder));
            app.UseAutofacMiddleware(autofac);
            app.UseAutofacMvc();

            // Sets the ASP.NET MVC dependency resolver.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(autofac));
        }
    }
}