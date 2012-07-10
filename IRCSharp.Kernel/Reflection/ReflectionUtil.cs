using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Reflection
{
	internal static class ReflectionUtil
	{
		internal static T LoadTypeOfInterfaceFromAssembly<T>(string dllPath) where T : class
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

		internal static T LoadTypeOf<T>(Type type) where T : class
		{
			return Activator.CreateInstance(type) as T;
		}

		internal static bool IsOfType<TInterface>(string dllPath)
		{
			bool isOfType = false;
			System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(dllPath);
			var types = assembly.GetTypes();

			foreach (var typeToLoad in types)
			{
				if (typeToLoad.GetInterfaces().Contains(typeof(TInterface)))
				{
					isOfType = true;
					break;
				}
			}

			return isOfType;
		}

		internal static Kernel.Model.Query.ResponseCommand GetIRCCommandName(string dllPath)
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(dllPath);
			Kernel.Model.Query.ResponseCommand name = Kernel.Model.Query.ResponseCommand.NOT_VALID_RESPONSE_COMMAND_TYPE;

			foreach (Type type in assembly.GetTypes())
			{
				if (type.GetCustomAttributes(typeof(IRCSharp.Kernel.IRCCommandAttribute), true).Count() > 0)
				{
					name = type.GetCustomAttributes(typeof(IRCSharp.Kernel.IRCCommandAttribute), true)
								   .Select(attribute => ((IRCSharp.Kernel.IRCCommandAttribute)attribute).Name)
								   .FirstOrDefault();

					break;
				}
			}

			if (name == null || name == Kernel.Model.Query.ResponseCommand.NOT_VALID_RESPONSE_COMMAND_TYPE)
				throw new Exception("Dll contains no names, or has a invalid name. This is not a command dll.");

			return name;
		}

		internal static Type GetTypeOf<TInterface>(string dllPath)
		{
			Type type = null;
			System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(dllPath);
			var types = assembly.GetTypes();

			foreach (var typeToLoad in types)
			{
				if (typeToLoad.GetInterfaces().Contains(typeof(TInterface)))
				{
					type = typeToLoad;
					break;
				}
			}

			return type;
		}

		internal static string GetUserdefinedName(string directoryPath)
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(directoryPath);
			string name = assembly.GetCustomAttributes(typeof(IRCSharp.Kernel.UserdefinedCommandAttribute), false)
							   .Select(attribute => ((IRCSharp.Kernel.UserdefinedCommandAttribute)attribute).Name)
							   .FirstOrDefault();

			if (name == null)
				throw new Exception("Dll contains no names. This is not a command dll.");

			return name;
		}
	}
}
