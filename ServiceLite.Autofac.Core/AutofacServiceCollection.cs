﻿using System;
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
        public void AddTransient<TContract, TService>() where TService : TContract
        {
            Builder.RegisterType<TService>().As<TContract>().InstancePerDependency();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddTransient<TService>(Func<IServiceProvider, TService> factory)
        {
            Builder.Register<TService>(context => factory(context.Resolve<IServiceProvider>())).InstancePerDependency();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddScoped<TContract, TService>() where TService : TContract
        {
            Builder.RegisterType<TService>().As<TContract>().InstancePerRequest();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddScoped<TService>(Func<IServiceProvider, TService> factory)
        {
            Builder.Register<TService>(context => factory(context.Resolve<IServiceProvider>())).InstancePerRequest();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddSingleton<TContract, TService>() where TService : TContract
        {
            Builder.RegisterType<TService>().As<TContract>().SingleInstance();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddSingleton<TService>(Func<IServiceProvider, TService> factory)
        {
            Builder.Register<TService>(context => factory(context.Resolve<IServiceProvider>())).SingleInstance();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public void AddSingleton<TService>(TService service)
        {
            Builder.Register<TService>(context => service).SingleInstance();
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public IServiceProvider Build() => Builder.Build().Resolve<IServiceProvider>();
    }
}