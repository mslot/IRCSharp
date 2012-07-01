using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	[AttributeUsage(AttributeTargets.Class)]
	public class IRCCommandAttribute : Attribute
	{
		public IRCSharp.Kernel.Query.ResponseCommand Name { get; private set; }

		public IRCCommandAttribute(IRCSharp.Kernel.Query.ResponseCommand name)
		{
			Name = name;
		}
	}
}
