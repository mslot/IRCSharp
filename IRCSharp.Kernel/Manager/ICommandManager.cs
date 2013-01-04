using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Manager
{
	public interface ICommandManager
	{
		void RemoveUserdefinedCommand(string name);
		List<CommandInformation<string>> GetUserdefinedCommand(string name);
		List<CommandInformation<Model.Query.IRCCommand>> GetIRCCommand(Kernel.Model.Query.IRCCommand type);
		void InsertIRCCommand(CommandInformation<Model.Query.IRCCommand> command);
		void InsertIRCCommand(Type commandType, Model.Query.IRCCommand name, string absoluteFilePath);
		void InsertUserdefinedCommand(CommandInformation<string> command);
		void InsertUserdefinedCommand(Type commandType, string name, string absoluteFilePath);
		List<Model.Query.IRCCommandQuery> FireUserdefinedCommand(IRCSharp.Kernel.Model.Query.IRCCommandQuery query);
		List<Model.Query.IRCCommandQuery> FireIRCCommand(IRCSharp.Kernel.Model.Query.IRCCommandQuery query);
		void InitAllCommands();
	}
}
