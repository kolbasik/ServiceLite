using DevdayDemo.ServiceLite.Autofac;
using DevdayDemo.ServiceLite.Core;
using DevdayDemo.ServiceLite.Features;
using DevdayDemo.Services;
using DevdayDemo.Services.Random;
using Owin;

namespace DevdayDemo
{
    public sealed class ServicesHost : AppHostBase
    {
        public ServicesHost()
        {
            Plugins.Add(new WebApiFeature());
            Plugins.Add(new AutofacWebApiFeature());
            Plugins.Add(new MvcFeature());
            Plugins.Add(new AutofacMvcFeature());
        }

        public AppHostBase Use(IAppBuilder app)
        {
            this.Set(app);
            this.Use(new AutofacServiceCollection());
            return this;
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
            container.AddScoped<IRandomService, RandomService>();
        }
    }
}