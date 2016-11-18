using System;
using System.ComponentModel.Design;

namespace DevdayDemo.ServiceLite.Core
{
    internal sealed class DefaultServiceCollection : IServiceCollection, IServiceProvider
    {
        private readonly ServiceContainer container;

        public DefaultServiceCollection()
        {
            container = new ServiceContainer();
        }

        public void AddTransient<TSource, TTarget>() where TTarget : TSource
        {
            container.AddService(typeof(TTarget), (sc, type) => Activator.CreateInstance(type));
        }

        public void AddSingleton<TSource, TTarget>() where TTarget : TSource
        {
            object singleton = null;
            container.AddService(typeof(TTarget), (sc, type) => singleton ?? (singleton = Activator.CreateInstance(type)));
        }

        public void AddInstance<TTarget>(TTarget target) where TTarget : class
        {
            container.AddService(typeof(TTarget), (sc, type) => target);
        }

        public void AddScoped<TSource, TTarget>() where TTarget : TSource
        {
            container.AddService(typeof(TTarget), (sc, type) => Activator.CreateInstance(type));
        }

        public IServiceProvider Build() => this;

        public object GetService(Type serviceType) => container.GetService(serviceType);
    }
}