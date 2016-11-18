namespace DevdayDemo.ServiceLite.Core
{
    public static class IAppHostExtensions
    {
        public static IAppHost Set<T>(this IAppHost appHost, string key, T value)
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

        public static IAppHost Set<T>(this IAppHost appHost, T value) => appHost.Set(typeof(T).FullName, value);
        public static T Get<T>(this IAppHost appHost) => appHost.Get<T>(typeof(T).FullName);
    }
}