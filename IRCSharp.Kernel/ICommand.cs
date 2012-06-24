using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	public interface ICommand<T,U> : IIdentifiable<T>
	{
		string Location { get; set; }
		Query.IRCCommandQuery Execute(U query);
	}
}
