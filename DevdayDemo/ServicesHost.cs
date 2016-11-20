using DevdayDemo.ServiceLite.Autofac;
using DevdayDemo.ServiceLite.Features;
using DevdayDemo.Services;
using DevdayDemo.Services.Random;
using Owin;
using ServiceLite.Core;
using ServiceLite.Swagger;

namespace DevdayDemo
{
    public sealed class ServicesHost : AppHostBase
    {
        public ServicesHost()
        {
            Plugins.Add(new AutofacMvcFeature());
            Plugins.Add(new AutofacWebApiFeature());
            Plugins.Add(new WebApiFeature());
            Plugins.Add(new SwaggerFeature { ApiVersions = { { "v1", "Application V1" }, { "v2", "Application V2" } } });
            Plugins.Add(new MvcFeature());
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
    }
}