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
            AddSingleton<IServiceProvider, AutofacServiceProvider>();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddTransient<TSource, TTarget>() where TTarget : TSource
        {
            Builder.RegisterType<TTarget>().As<TSource>().InstancePerDependency();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddTransient<TSource>(Func<IServiceProvider, TSource> factory)
        {
            Builder.Register<TSource>(context => factory(context.Resolve<IServiceProvider>())).InstancePerDependency();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddScoped<TSource, TTarget>() where TTarget : TSource
        {
            Builder.RegisterType<TTarget>().As<TSource>().InstancePerRequest();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddScoped<TSource>(Func<IServiceProvider, TSource> factory)
        {
            Builder.Register<TSource>(context => factory(context.Resolve<IServiceProvider>())).InstancePerRequest();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddSingleton<TSource, TTarget>() where TTarget : TSource
        {
            Builder.RegisterType<TTarget>().As<TSource>().SingleInstance();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddSingleton<TSource>(Func<IServiceProvider, TSource> factory)
        {
            Builder.Register<TSource>(context => factory(context.Resolve<IServiceProvider>())).SingleInstance();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddInstance<TTarget>(TTarget target) where TTarget : class
        {
            Builder.RegisterInstance(target);
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public IServiceProvider Build() => Builder.Build().Resolve<IServiceProvider>();
    }
}