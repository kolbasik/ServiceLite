using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevdayDemo.Services;
using DevdayDemo.Services.Random;
using Ninject.Web.Mvc;
using Owin;
using ServiceLite.Autofac.Core;
using ServiceLite.Autofac.Mvc5;
using ServiceLite.Autofac.WebApi2;
using ServiceLite.Core;
using ServiceLite.Mvc5;
using ServiceLite.NInject.Core;
using ServiceLite.NInject.Mvc5;
using ServiceLite.Swagger;
using ServiceLite.WebApi2;

namespace DevdayDemo
{
    internal sealed class AppHost : AppHostBase, IStartable
    {
        public AppHost()
        {
            Plugins.Add(new NInjectFeature());
            Plugins.Add(new NInjectMvc5Feature());
            //Plugins.Add(new AutofacFeature());
            //Plugins.Add(new AutofacWebApiFeature());
            //Plugins.Add(new AutofacMvcFeature());
            Plugins.Add(new WebApiFeature());
            Plugins.Add(new SwaggerFeature { ApiVersions = { { "v1", "Application V1" }, { "v2", "Application V2" } } });
            Plugins.Add(new MvcFeature { RegisterRoutesEnabled = false });
        }

        //public AppHostBase Run(IAppBuilder app) => this.Set(app).Configure(new AutofacServiceCollection()).Start();
        public AppHostBase Run(IAppBuilder app) => this.Set(app).Configure(new NInjectServiceCollection { InRequestScope = x => HttpContext.Current }).Start();

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<IBrowserConfigService, BrowserConfigService>();
            services.AddScoped<IFeedService, FeedService>();
            services.AddScoped<IManifestService, ManifestService>();
            services.AddScoped<IOpenSearchService, OpenSearchService>();
            services.AddScoped<IRobotsService, RobotsService>();
            services.AddScoped<ISitemapService, SitemapService>();
            services.AddScoped<ISitemapPingerService, SitemapPingerService>();
            services.AddSingleton<IRandomService>(new RandomService());
        }

        public void Start(StartContext context)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}