using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Collections
{
	class IRCCommandCollection : SynchronizedDictionary<Kernel.Model.Query.ResponseCommand, List<CommandInformation<Model.Query.ResponseCommand>>>
	{
	}
}
