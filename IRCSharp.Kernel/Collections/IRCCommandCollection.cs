using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Collections
{
	class IRCCommandCollection : SynchronizedDictionary<Kernel.Query.ResponseCommand, List<CommandInformation<Query.ResponseCommand>>>
	{
	}
}
