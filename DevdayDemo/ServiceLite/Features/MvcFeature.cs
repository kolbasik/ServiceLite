﻿using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Boilerplate.Web.Mvc;
using DevdayDemo.ServiceLite.Core;

namespace DevdayDemo.ServiceLite.Features
{
    public sealed class MvcFeature : Plugin
    {
        public override void Register(IAppHost appHost)
        {
            // Ensure that the X-AspNetMvc-Version HTTP header is not
            MvcHandler.DisableMvcResponseHeader = true;

            ConfigureViewEngines(ViewEngines.Engines);
            ConfigureAntiForgeryTokens();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        /// <summary>
        ///     Configures the view engines. By default, Asp.Net MVC includes the Web Forms (WebFormsViewEngine) and
        ///     Razor (RazorViewEngine) view engines that supports both C# (.cshtml) and VB (.vbhtml). You can remove view
        ///     engines you are not using here for better performance and include a custom Razor view engine that only
        ///     supports C#.
        /// </summary>
        private static void ConfigureViewEngines(ViewEngineCollection viewEngines)
        {
            viewEngines.Clear();
            viewEngines.Add(new CSharpRazorViewEngine());
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