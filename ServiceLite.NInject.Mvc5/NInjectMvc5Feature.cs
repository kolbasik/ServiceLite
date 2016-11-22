using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using ServiceLite.Core;

namespace ServiceLite.NInject.Mvc5
{
    public sealed class NInjectMvc5Feature : IPlugin, IConfigurable
    {
        public void Configure(ConfigurationContext context)
        {
            var services = context.Services;
            services.AddScoped<HttpContextBase>(sp => new HttpContextWrapper(HttpContext.Current));
            services.AddScoped<HttpRequestBase>(sp => sp.GetRequiredService<HttpContextBase>().Request);
            services.AddScoped<HttpResponseBase>(sp => sp.GetRequiredService<HttpContextBase>().Response);
            services.AddScoped<HttpServerUtilityBase>(sp => sp.GetRequiredService<HttpContextBase>().Server);
            services.AddScoped<HttpSessionStateBase>(sp => sp.GetRequiredService<HttpContextBase>().Session);
            services.AddScoped<HttpApplicationStateBase>(sp => sp.GetRequiredService<HttpContextBase>().Application);
            services.AddScoped<HttpBrowserCapabilitiesBase>(sp => sp.GetRequiredService<HttpRequestBase>().Browser);
            services.AddScoped<HttpFileCollectionBase>(sp => sp.GetRequiredService<HttpRequestBase>().Files);
            services.AddScoped<RequestContext>(sp => sp.GetRequiredService<HttpRequestBase>().RequestContext);
            services.AddScoped<HttpCachePolicyBase>(sp => sp.GetRequiredService<HttpResponseBase>().Cache);
            services.AddScoped<VirtualPathProvider>(sp => HostingEnvironment.VirtualPathProvider);
            services.AddScoped<UrlHelper>(sp => new UrlHelper(sp.GetRequiredService<RequestContext>()));
        }

        public void Start(StartContext context)
        {
        }
    }
}