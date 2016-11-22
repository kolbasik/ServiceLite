using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ServiceLite.Core;

namespace ServiceLite.Mvc5
{
    public sealed class MvcFeature : IPlugin, IConfigurable
    {
        public MvcFeature()
        {
            RegisterUrlHelper = true;
            RegisterAllAreasEnabled = true;
            RegisterRoutesEnabled = true;
            RegisterBundlesEnabled = true;
            RegisterGlobalFiltersEnabled = true;

            IgnoreAxdRoutes = true;
            UseMvcAttributeRoutes = true;
            UseDefaultRoutes = true;

            BundlesUseCdn = true;

            GlobalFilters = new List<FilterAttribute>();
        }

        public bool RegisterUrlHelper { get; set; }
        public bool RegisterAllAreasEnabled { get; set; }
        public bool RegisterRoutesEnabled { get; set; }
        public bool RegisterBundlesEnabled { get; set; }
        public bool RegisterGlobalFiltersEnabled { get; set; }

        public bool IgnoreAxdRoutes { get; set; }
        public bool UseMvcAttributeRoutes { get; set; }
        public bool UseDefaultRoutes { get; set; }

        public bool? BundlesEnableOptimizations { get; set; }
        public bool BundlesUseCdn { get; set; }

        public List<FilterAttribute> GlobalFilters { get; }


        public void Configure(ConfigurationContext context)
        {
            var services = context.Services;

            if (RegisterUrlHelper)
            {
                services.AddScoped<UrlHelper>(sp => new UrlHelper(sp.GetRequiredService<RequestContext>()));
            }
        }

        public void Start(StartContext context)
        {
            // Ensure that the X-AspNetMvc-Version HTTP header is not
            MvcHandler.DisableMvcResponseHeader = true;

            ConfigureViewEngines(ViewEngines.Engines);
            ConfigureAntiForgeryTokens();

            if (RegisterAllAreasEnabled) AreaRegistration.RegisterAllAreas();
            if (RegisterRoutesEnabled) RegisterRoutes(RouteTable.Routes);
            if (RegisterBundlesEnabled) RegisterBundles(BundleTable.Bundles);
            if (RegisterGlobalFiltersEnabled) RegisterGlobalFilters(System.Web.Mvc.GlobalFilters.Filters);
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            // Improve SEO by stopping duplicate URL's due to case differences or trailing slashes.
            // See http://googlewebmastercentral.blogspot.co.uk/2010/04/to-slash-or-not-to-slash.html
            routes.AppendTrailingSlash = true;
            routes.LowercaseUrls = true;

            if (IgnoreAxdRoutes)
            {
                // IgnoreRoute - Tell the routing system to ignore certain routes for better performance.
                // Ignore .axd files.
                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            }

            if (UseMvcAttributeRoutes)
            {
                // Enable attribute routing.
                routes.MapMvcAttributeRoutes();
            }

            if (UseDefaultRoutes)
            {
                // Normal routes are not required because we are using attribute routing. So we don't need this MapRoute
                // statement. Unfortunately, Elmah.MVC has a bug in which some 404 and 500 errors are not logged without
                // this route in place. So we include this but look out on these pages for a fix:
                // https://github.com/alexbeletsky/elmah-mvc/issues/60
                // https://github.com/ASP-NET-MVC-Boilerplate/Templates/issues/8
                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            }
        }

        /// <summary>
        /// For more information on bundling, visit <see cref="http://go.microsoft.com/fwlink/?LinkId=301862"/>.
        /// </summary>
        public void RegisterBundles(BundleCollection bundles)
        {
            // Enable Optimizations
            // Set EnableOptimizations to false for debugging. For more information,
            // Web.config file system.web/compilation[debug=true]
            // OR
            if (BundlesEnableOptimizations.HasValue)
                BundleTable.EnableOptimizations = BundlesEnableOptimizations.Value;

            // Enable CDN usage.
            // Note: that you can choose to remove the CDN if you are developing an intranet application.
            // Note: We are using Google's CDN where possible and then Microsoft if not available for better
            //       performance (Google is more likely to have been cached by the users browser).
            // Note: that protocol (http:) is omitted from the CDN URL on purpose to allow the browser to choose the protocol.
            bundles.UseCdn = BundlesUseCdn;
        }

        public void RegisterGlobalFilters(GlobalFilterCollection filters) => GlobalFilters.ForEach(filters.Add);

        /// <summary>
        ///     Configures the view engines. By default, Asp.Net MVC includes the Web Forms (WebFormsViewEngine) and
        ///     Razor (RazorViewEngine) view engines that supports both C# (.cshtml) and VB (.vbhtml). You can remove view
        ///     engines you are not using here for better performance and include a custom Razor view engine that only
        ///     supports C#.
        /// </summary>
        private static void ConfigureViewEngines(ViewEngineCollection viewEngines)
        {
            viewEngines.Clear();
            viewEngines.Add(new RazorViewEngine()); // NOTE: Boilerplate.Web.Mvc5.CSharpRazorViewEngine is .cshtml only optimization.
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
    }
}