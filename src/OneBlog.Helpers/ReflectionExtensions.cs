using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OneBlog.Helpers
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<T> GetTypes<T>(this Assembly assembly)
        {
            var result = new List<T>();

            var types = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && typeof(T).IsAssignableFrom(t))
                .ToList();

            foreach (var type in types)
            {
                var instance = (T)Activator.CreateInstance(type);
                result.Add(instance);
            }

            return result;
        }


    }
}
