using System;
using System.Diagnostics;

namespace ServiceLite.Core
{
    public static class IAppHostExtensions
    {
        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public static TAppHost Set<TAppHost, TValue>(this TAppHost appHost, string key, TValue value) where TAppHost : IAppHost
        {
            appHost.Properties[key] = value;
            return appHost;
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public static T Get<T>(this IAppHost appHost, string key, T defaultValue = default(T))
        {
            object value;
            if (appHost.Properties.TryGetValue(key, out value))
                return (T) value;
            return defaultValue;
        }

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public static TAppHost Set<TAppHost, TValue>(this TAppHost appHost, TValue value) where TAppHost : IAppHost
            => appHost.Set(typeof(TValue).FullName, value);

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public static T Get<T>(this IAppHost appHost)
        {
            var valueName = typeof(T).FullName;
            var value = appHost.Get<T>(valueName);
            if (object.Equals(value, default(T)))
            {
                throw new ArgumentNullException(valueName, "Could not find the value.");
            }
            return value;
        }
    }
}