namespace DevdayDemo.ServiceLite.Core
{
    public static class IAppHostExtensions
    {
        public static TAppHost Set<TAppHost, TValue>(this TAppHost appHost, string key, TValue value) where TAppHost : IAppHost
        {
            appHost.Properties[key] = value;
            return appHost;
        }

        public static T Get<T>(this IAppHost appHost, string key, T defaultValue = default(T))
        {
            object value;
            if (appHost.Properties.TryGetValue(key, out value))
                return (T) value;
            return defaultValue;
        }

        public static TAppHost Set<TAppHost, TValue>(this TAppHost appHost, TValue value) where TAppHost : IAppHost
            => appHost.Set(typeof(TValue).FullName, value);

        public static T Get<T>(this IAppHost appHost)
            => appHost.Get<T>(typeof(T).FullName);
    }
}