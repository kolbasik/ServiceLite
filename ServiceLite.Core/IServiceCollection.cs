using System;

namespace ServiceLite.Core
{
    public interface IServiceCollection
    {
        void AddTransient<TContract, TService>() where TService : TContract;
        void AddTransient<TService>(Func<IServiceProvider, TService> factory);
        void AddScoped<TContract, TService>() where TService : TContract;
        void AddScoped<TService>(Func<IServiceProvider, TService> factory);
        void AddSingleton<TContract, TService>() where TService : TContract;
        void AddSingleton<TService>(Func<IServiceProvider, TService> factory);
        void AddSingleton<TService>(TService target);
        IServiceProvider Build();
    }
}