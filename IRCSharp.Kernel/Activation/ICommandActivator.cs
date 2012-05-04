using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Activation
{
	public interface ICommandActivator
	{
		string Invoke(Query.IRCCommandQuery query);
	}
}
