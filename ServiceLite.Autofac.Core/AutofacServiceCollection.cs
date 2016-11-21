using System;
using System.Diagnostics;
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

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddTransient<TSource, TTarget>() where TTarget : TSource
        {
            Builder.RegisterType<TTarget>().As<TSource>().InstancePerDependency();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddSingleton<TSource, TTarget>() where TTarget : TSource
        {
            Builder.RegisterType<TTarget>().As<TSource>().SingleInstance();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddInstance<TTarget>(TTarget target) where TTarget : class
        {
            Builder.RegisterInstance(target);
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddScoped<TSource, TTarget>() where TTarget : TSource
        {
            Builder.RegisterType<TTarget>().As<TSource>().InstancePerRequest();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public IServiceProvider Build() => new AutofacServiceProvider(Builder.Build());
    }
}