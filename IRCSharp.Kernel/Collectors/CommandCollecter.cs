using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Collecters
{
	public class CommandCollecter
	{
		private Kernel.Watchers.DllFileWatcher _dllWatcher = null;
		public Manager.CommandManager CommandManager { get; private set; }

		public CommandCollecter(string directoryPath)
		{
			CommandManager = new Manager.CommandManager();
			InitDirectory(directoryPath);
			_dllWatcher = new Watchers.DllFileWatcher(directoryPath);
			_dllWatcher.Changed += new System.IO.FileSystemEventHandler(_dllWatcher_Changed);
			_dllWatcher.Created += new System.IO.FileSystemEventHandler(_dllWatcher_Created);
			_dllWatcher.Deleted += new System.IO.FileSystemEventHandler(_dllWatcher_Deleted);
			_dllWatcher.Renamed += new System.IO.RenamedEventHandler(_dllWatcher_Renamed);

		}

		public void Start()
		{
			_dllWatcher.Start();
		}

		private void InitDirectory(string directoryPath)
		{
			foreach (string absoluteFilePath in System.IO.Directory.GetFiles(directoryPath))
			{
				Type commandType = null;
				if (Reflection.ReflectionUtil.IsOfType<ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>>(absoluteFilePath))
				{
					commandType = Reflection.ReflectionUtil.GetTypeOf<ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>>(absoluteFilePath);
					string name = Reflection.ReflectionUtil.GetUserdefinedName(directoryPath);
					CommandInformation<string> information = new CommandInformation<string>(commandType, name, absoluteFilePath);

					CommandManager.InsertUserdefinedCommand(information,name);

					ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>>(commandType);
					command.Init(); //TODO: Move this init out! This should not be the responsibility of the command collecter!!!
				}
				else if (Reflection.ReflectionUtil.IsOfType<ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>>(absoluteFilePath))
				{
					commandType = Reflection.ReflectionUtil.GetTypeOf<ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>>(absoluteFilePath);
					Query.ResponseCommand name = Reflection.ReflectionUtil.GetIRCCommandName(absoluteFilePath);
					CommandInformation<Query.ResponseCommand> information = new CommandInformation<Query.ResponseCommand>(commandType, name, absoluteFilePath);

					CommandManager.InsertIRCCommand(information, name);

					ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>>(commandType);
					command.Init(); //TODO: Move this init out! This should not be the responsibility of the command collecter!!!
				}


			}
		}

		public void Stop()
		{
			_dllWatcher.Stop();
		}

		private void _dllWatcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
		{
			//TODO take action here. The dll is in fact not removed from the AppDomain. This can never be done.
		}

		private void _dllWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
		{
			//TODO: should it be possible to insert IRC commands on the run? Why? Why not?
			string absoluteFilePath = e.FullPath;
			if (Reflection.ReflectionUtil.IsOfType<ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>>(absoluteFilePath))
			{
				Type commandType = Reflection.ReflectionUtil.GetTypeOf < ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>>(absoluteFilePath);
				string name = Reflection.ReflectionUtil.GetUserdefinedName(absoluteFilePath);

				CommandInformation<string> information = new CommandInformation<string>(commandType, name, absoluteFilePath);

				CommandManager.InsertUserdefinedCommand(information, name);
			}
		}

		private void _dllWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
		{
			//TODO: implement this
		}

		private void _dllWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
		{
			//TODO: implement this
		}
	}
}
