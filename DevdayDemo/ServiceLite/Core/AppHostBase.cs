using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DevdayDemo.ServiceLite.Core
{
    public abstract class AppHostBase : IAppHost
    {
        public static AppHostBase Instance;

        public AppHostBase()
        {
            Plugins = new List<Plugin>();
            Instance = this;
        }

        public List<Plugin> Plugins { get; }
        public IServiceProvider Container { get; private set; }

        public virtual AppHostBase Use(IServiceCollection container)
        {
            Configure(container);
            ConfigurePlugins(container);
            Container = container.Build();
            return this;
        }

        public virtual AppHostBase Start()
        {
            if (Container == null) throw new NotSupportedException("Please ensure that .Use(IServiceCollection container) method was called.");
            RegisterPlugins();
            return this;
        }

        protected virtual void Configure(IServiceCollection container)
        {
        }

        protected virtual void ConfigurePlugins(IServiceCollection container)
        {
            foreach (var plugin in Plugins)
                try
                {
                    plugin.Configure(container);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                }
        }

        protected virtual void RegisterPlugins()
        {
            foreach (var plugin in Plugins)
                try
                {
                    plugin.Register(this);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                }
        }
    }
}