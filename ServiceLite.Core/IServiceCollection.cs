using System;

namespace ServiceLite.Core
{
    public interface IServiceCollection
    {
        void AddTransient<TSource, TTarget>() where TTarget : TSource;
        void AddSingleton<TSource, TTarget>() where TTarget : TSource;
        void AddInstance<TTarget>(TTarget target) where TTarget : class;
        void AddScoped<TSource, TTarget>() where TTarget : TSource;
        IServiceProvider Build();
    }
}