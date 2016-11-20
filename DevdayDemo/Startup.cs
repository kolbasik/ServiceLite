using DevdayDemo;
using DevdayDemo.ServiceLite.Autofac;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DevdayDemo
{
    public sealed class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            new ServicesHost().Run(app);
        }
    }
}