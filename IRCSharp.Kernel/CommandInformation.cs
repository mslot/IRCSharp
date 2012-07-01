using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	public class CommandInformation<T>
	{
		public Type CommandType { get; private set; }
		public T Name { get; private set; }
		public string Path {get; private set;}

		public CommandInformation(Type commandType, T name, string path)
		{
			CommandType = commandType;
			Name = name;
			Path = path;
		}
	}
}
