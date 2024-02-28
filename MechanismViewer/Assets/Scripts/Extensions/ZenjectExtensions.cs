using System;
using System.Linq;
using Zenject;

namespace Extensions
{
    public static class ZenjectExtensions
    {
        public static void BindAllImplementationsOfType<T>(this DiContainer container, bool bindInterfaces = true)
        {
            var implementations = AppDomain.CurrentDomain
                                           .GetAssemblies()
                                           .SelectMany(s => s.GetTypes())
                                           .Where(type => typeof(T).IsAssignableFrom(type) 
                                                          && !type.IsAbstract 
                                                          && !type.IsGenericType)
                                           .ToArray();
            
            container.Bind<T>().To(implementations).AsCached();
            if (!bindInterfaces) 
                return;
            
            for (int i = implementations.Length - 1; i >= 0; i--)
            {
                container.BindInterfacesTo(implementations[i]).AsCached().IfNotBound();
            }
        }
    }
}