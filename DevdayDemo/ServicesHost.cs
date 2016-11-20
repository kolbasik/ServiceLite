using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevdayDemo.Services;
using DevdayDemo.Services.Random;
using Owin;
using ServiceLite.Autofac.Core;
using ServiceLite.Autofac.Mvc5;
using ServiceLite.Autofac.WebApi2;
using ServiceLite.Core;
using ServiceLite.Mvc;
using ServiceLite.Swagger;
using ServiceLite.WebApi;

namespace DevdayDemo
{
    public sealed class ServicesHost : AppHostBase
    {
        public ServicesHost()
        {
            Plugins.Add(new AutofacWebApiFeature());
            Plugins.Add(new WebApiFeature());
            Plugins.Add(new SwaggerFeature { ApiVersions = { { "v1", "Application V1" }, { "v2", "Application V2" } } });
            Plugins.Add(new AutofacMvcFeature());
            Plugins.Add(new MvcFeature { RegisterRoutesEnabled = false });
            Plugins.Add(new CustomFeature());
        }

        public AppHostBase Run(IAppBuilder app) => this.Set(app).Configure(new AutofacServiceCollection()).Start();

        protected override void ConfigureServices(IServiceCollection container)
        {
            container.AddSingleton<ILoggingService, LoggingService>();
            container.AddSingleton<ICacheService, CacheService>();
            container.AddScoped<IBrowserConfigService, BrowserConfigService>();
            container.AddScoped<IFeedService, FeedService>();
            container.AddScoped<IManifestService, ManifestService>();
            container.AddScoped<IOpenSearchService, OpenSearchService>();
            container.AddScoped<IRobotsService, RobotsService>();
            container.AddScoped<ISitemapService, SitemapService>();
            container.AddScoped<ISitemapPingerService, SitemapPingerService>();
            container.AddScoped<IRandomService, RandomService>();
        }

        private sealed class CustomFeature : IPlugin
        {
            public void Start(StartContext context)
            {
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            }
        }
    }
}