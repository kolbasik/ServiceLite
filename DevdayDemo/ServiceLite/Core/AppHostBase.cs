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
            Config = new HostConfig();
            Plugins = new List<IPlugin>();
        }

        public HostConfig Config { get; }
        public List<IPlugin> Plugins { get; }
        public IServiceProvider Container { get; private set; }

        public virtual AppHostBase Use(IServiceCollection container)
        {
            this.Configure(container);
            this.ConfigurePlugins(container);
            Container = container.Build();
            return this;
        }

        public AppHostBase Init()
        {
            InitPlugins();
            Instance = this;
            return this;
        }

        protected virtual void Configure(IServiceCollection container)
        {
        }

        protected virtual void ConfigurePlugins(IServiceCollection container)
        {
            foreach (IPlugin plugin in this.Plugins)
            {
                try
                {
                    plugin.Configure(container);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        protected virtual void InitPlugins()
        {
            foreach (IPlugin plugin in this.Plugins)
            {
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

    public interface IAppHost
    {
        HostConfig Config { get; }
        List<IPlugin> Plugins { get; }
        IServiceProvider Container { get; }
    }

    public interface IPlugin
    {
        void Configure(IServiceCollection container);
        void Register(IAppHost appHost);
    }

    public sealed class HostConfig
    {
    }

    public interface IServiceCollection
    {
        void AddTransient<TSource, TTarget>() where TTarget : TSource;
        void AddSingleton<TSource, TTarget>() where TTarget : TSource;
        void AddInstance<TTarget>(TTarget target) where TTarget : class;
        void AddScoped<TSource, TTarget>() where TTarget : TSource;
        IServiceProvider Build();
    }
}