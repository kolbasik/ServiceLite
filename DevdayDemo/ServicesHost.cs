using DevdayDemo.ServiceLite.Core;
using DevdayDemo.ServiceLite.Features;
using DevdayDemo.Services;

namespace DevdayDemo
{
    public sealed class ServicesHost : AppHostBase
    {
        public ServicesHost()
        {
            Plugins.Add(new MvcFeature());
            Plugins.Add(new AutofacMvcFeature());
        }

        protected override void Configure(IServiceCollection container)
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
        }
    }
}