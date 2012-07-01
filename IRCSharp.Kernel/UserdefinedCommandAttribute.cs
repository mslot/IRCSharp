using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	[AttributeUsage(AttributeTargets.Class)]
	public class UserdefinedCommandAttribute : Attribute
	{
		public string Name { get; private set; }

		public UserdefinedCommandAttribute(string name)
		{
			Name = name;
		}
	}
}
