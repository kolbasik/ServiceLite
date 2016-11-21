using DevdayDemo;
using JetBrains.Annotations;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DevdayDemo
{
    [PublicAPI]
    public sealed class Startup
    {
        public void Configuration(IAppBuilder app) => new AppHost().Run(app);
    }
}