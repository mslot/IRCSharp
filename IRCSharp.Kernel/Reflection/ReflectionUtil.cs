using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Reflection
{
    public static class ReflectionUtil
    {
        public static T LoadTypeOfInterfaceFromAssembly<T>(string dllPath) where T : class
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(dllPath);
            var types = assembly.GetTypes();

            T type = default(T);
            foreach (var typeToLoad in types)
            {
                if (typeToLoad.GetInterfaces().Contains(typeof(T)))
                {
                    type = Activator.CreateInstance(typeToLoad) as T;
                    break;
                }
            }

            return type;
        }
    }
}
