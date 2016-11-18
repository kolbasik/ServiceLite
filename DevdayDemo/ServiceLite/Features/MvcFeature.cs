using System.Reflection;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Boilerplate.Web.Mvc;
using DevdayDemo.ServiceLite.Autofac;
using DevdayDemo.ServiceLite.Core;
using Owin;

namespace DevdayDemo.ServiceLite.Features
{
    public sealed class MvcFeature : IPlugin
    {
        public void Configure(IServiceCollection container)
        {
            var builder = ((AutofacServiceCollection) container).Builder;

            // Register Common MVC Types
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register MVC Filters
            builder.RegisterFilterProvider();

            // Register MVC Controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
        }

        public void Register(IAppHost appHost)
        {
            // Ensure that the X-AspNetMvc-Version HTTP header is not
            //MvcHandler.DisableMvcResponseHeader = true;

            ConfigureViewEngines();
            ConfigureAntiForgeryTokens();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            var autofac = ((AutofacServiceProvider) appHost.Container).Container;
            SetMvcDependencyResolver(autofac);

            var app = (IAppBuilder) appHost.Container.GetService(typeof(IAppBuilder));
            app.UseAutofacMiddleware(autofac);
            app.UseAutofacMvc();
        }

        /// <summary>
        ///     Configures the view engines. By default, Asp.Net MVC includes the Web Forms (WebFormsViewEngine) and
        ///     Razor (RazorViewEngine) view engines that supports both C# (.cshtml) and VB (.vbhtml). You can remove view
        ///     engines you are not using here for better performance and include a custom Razor view engine that only
        ///     supports C#.
        /// </summary>
        private static void ConfigureViewEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());
        }

        /// <summary>
        ///     Configures the anti-forgery tokens. See
        ///     http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages
        /// </summary>
        private static void ConfigureAntiForgeryTokens()
        {
            // Rename the Anti-Forgery cookie from "__RequestVerificationToken" to "f". This adds a little security
            // through obscurity and also saves sending a few characters over the wire. Sadly there is no way to change
            // the form input name which is hard coded in the @Html.AntiForgeryToken helper and the
            // ValidationAntiforgeryTokenAttribute to  __RequestVerificationToken.
            // <input name="__RequestVerificationToken" type="hidden" value="..." />
            AntiForgeryConfig.CookieName = "f";

            // If you have enabled SSL. Uncomment this line to ensure that the Anti-Forgery
            // cookie requires SSL to be sent across the wire.
            // AntiForgeryConfig.RequireSsl = true;
        }

        /// <summary>
        /// Sets the ASP.NET MVC dependency resolver.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void SetMvcDependencyResolver(IContainer container)
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}