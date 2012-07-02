using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Collections
{
	class UserdefinedCommandCollection : SynchronizedDictionary<string, List<CommandInformation<string>>>
	{
	}
}
