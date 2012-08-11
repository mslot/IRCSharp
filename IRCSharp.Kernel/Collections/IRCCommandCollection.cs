using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Collections
{
	class IRCCommandCollection : SynchronizedDictionary<Kernel.Model.Query.IRCCommand, List<CommandInformation<Model.Query.IRCCommand>>>
	{
	}
}
