using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DevdayDemo.ServiceLite
{
    public abstract class AppHostBase : IAppHost
    {
        public static AppHostBase Instance;

        public AppHostBase()
        {
            //new ServiceContainer()
            Config = new HostConfig();
            Plugins = new List<IPlugin>();
        }

        public HostConfig Config { get; }
        public List<IPlugin> Plugins { get; }
        public IServiceProvider Container { get; private set; }

        public virtual void Use(IServiceCollection serviceCollection)
        {
            this.Configure(serviceCollection);
            Container = serviceCollection.Build();
        }

        public virtual IAppHost Init()
        {
            this.ConfigurePlugins();
            Instance = this;
            return this;
        }

        protected virtual void Configure(IServiceCollection container)
        {
        }

        protected virtual void ConfigurePlugins()
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
        void Register(IAppHost appHost);
    }

    public sealed class HostConfig
    {
    }

    public interface IServiceCollection
    {
        void AddTransient<TSource, TTarget>() where TTarget : TSource;
        void AddSingleton<TSource, TTarget>() where TTarget : TSource;
        void AddInstance<TTarget>(TTarget target);
        void AddScoped<TSource, TTarget>() where TTarget : TSource;
        IServiceProvider Build();
    }
}