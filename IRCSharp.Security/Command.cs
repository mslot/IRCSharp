using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Security
{
	public class Command
	{
		public string Name { get; private set; }
		public List<CommandRight> Rights { get; private set; }

		public Command(string name, string rights)
		{
			Name = name;
			Rights = new List<CommandRight>();
			Rights = CompileRights(rights);
		}

		private List<CommandRight> CompileRights(string rights)
		{
			List<CommandRight> convertedRights = new List<CommandRight>();
			if (rights != null)
			{
				string[] rightsSplitted = rights.Split('|');
				convertedRights = new List<CommandRight>(rightsSplitted.Count());

				foreach (string right in rightsSplitted)
				{
					CommandRight convertedRight = new CommandRight(right);
					convertedRights.Add(convertedRight);
				}
			}
			return convertedRights;
		}
	}
}
