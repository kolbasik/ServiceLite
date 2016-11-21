using System;

namespace ServiceLite.Core
{
    public interface IServiceCollection
    {
        void AddTransient<TSource, TTarget>() where TTarget : TSource;
        void AddTransient<TSource>(Func<IServiceProvider, TSource> factory);
        void AddScoped<TSource, TTarget>() where TTarget : TSource;
        void AddScoped<TSource>(Func<IServiceProvider, TSource> factory);
        void AddSingleton<TSource, TTarget>() where TTarget : TSource;
        void AddSingleton<TSource>(Func<IServiceProvider, TSource> factory);
        void AddInstance<TTarget>(TTarget target) where TTarget : class;
        IServiceProvider Build();
    }
}