using System;
using System.Diagnostics;

namespace ServiceLite.Core
{
    public static class AppHostExtensions
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
        public static T GetOptional<T>(this IAppHost appHost) => appHost.Get<T>(typeof(T).FullName);

        [DebuggerHidden, DebuggerNonUserCode, DebuggerStepThrough]
        public static T GetRequired<T>(this IAppHost appHost)
        {
            var value = appHost.GetOptional<T>();
            if (object.Equals(value, default(T)))
            {
                throw new InvalidOperationException($@"Could not find the value of '{typeof(T).FullName}' type.");
            }
            return value;
        }
    }
}