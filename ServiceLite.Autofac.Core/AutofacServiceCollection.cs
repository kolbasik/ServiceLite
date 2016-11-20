using System;
using Autofac;
using ServiceLite.Core;

namespace ServiceLite.Autofac.Core
{
    public sealed class AutofacServiceCollection : IServiceCollection
    {
        public readonly ContainerBuilder Builder;

        public AutofacServiceCollection()
        {
            Builder = new ContainerBuilder();
        }

        public void AddTransient<TSource, TTarget>() where TTarget : TSource
        {
            Builder.RegisterType<TTarget>().As<TSource>().InstancePerDependency();
        }

        public void AddSingleton<TSource, TTarget>() where TTarget : TSource
        {
            Builder.RegisterType<TTarget>().As<TSource>().SingleInstance();
        }

        public void AddInstance<TTarget>(TTarget target) where TTarget : class
        {
            Builder.RegisterInstance(target);
        }

        public void AddScoped<TSource, TTarget>() where TTarget : TSource
        {
            Builder.RegisterType<TTarget>().As<TSource>().InstancePerRequest();
        }

        public IServiceProvider Build() => new AutofacServiceProvider(Builder.Build());
    }
}