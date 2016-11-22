using System;
using System.Diagnostics;
using Ninject;
using Ninject.Activation;
using ServiceLite.Core;

namespace ServiceLite.NInject.Core
{
    public sealed class NInjectServiceCollection : IServiceCollection
    {
        public readonly IKernel Kernel;

        public NInjectServiceCollection(IKernel kernel, Func<IContext, object> getScope)
        {
            if (kernel == null)
                throw new ArgumentNullException(nameof(kernel));
            if (getScope == null)
                throw new ArgumentNullException(nameof(getScope));
            Kernel = kernel;
            GetScope = getScope;
        }

        public Func<IContext, object> GetScope { get; }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        [DebuggerStepThrough]
        public void AddTransient<TContract, TService>() where TService : TContract
        {
            Kernel.Bind<TContract>().To<TService>().InTransientScope();
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        [DebuggerStepThrough]
        public void AddTransient<TService>(Func<IServiceProvider, TService> factory)
        {
            Kernel.Bind<TService>().ToMethod(context => factory(context.Kernel)).InTransientScope();
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        [DebuggerStepThrough]
        public void AddScoped<TContract, TService>() where TService : TContract
        {
            Kernel.Bind<TContract>().To<TService>().InScope(GetScope);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        [DebuggerStepThrough]
        public void AddScoped<TService>(Func<IServiceProvider, TService> factory)
        {
            Kernel.Bind<TService>().ToMethod(context => factory(context.Kernel)).InScope(GetScope);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        [DebuggerStepThrough]
        public void AddSingleton<TContract, TService>() where TService : TContract
        {
            Kernel.Bind<TContract>().To<TService>().InSingletonScope();
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        [DebuggerStepThrough]
        public void AddSingleton<TService>(Func<IServiceProvider, TService> factory)
        {
            Kernel.Bind<TService>().ToMethod(context => factory(context.Kernel)).InSingletonScope();
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        [DebuggerStepThrough]
        public void AddSingleton<TService>(TService service)
        {
            Kernel.Bind<TService>().ToConstant(service);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        [DebuggerStepThrough]
        public IServiceProvider Build() => Kernel;
    }
}