using DevdayDemo.ServiceLite.Autofac;

[assembly: Microsoft.Owin.OwinStartup(typeof(DevdayDemo.Startup))]

namespace DevdayDemo
{
    using System.Web.Mvc;
    using NWebsec.Owin;
    using Owin;

    public sealed class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var autofac = new AutofacServiceCollection();
            autofac.AddInstance(app);
            new ServicesHost().Use(autofac).Init();
        }
    }
}
