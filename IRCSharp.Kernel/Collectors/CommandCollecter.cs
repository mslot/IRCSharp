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

		public void Stop()
		{
			_dllWatcher.Stop();
		}

		void _dllWatcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
		{
			ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOfInterfaceFromAssembly<ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>>(e.FullPath);
			CommandManager.RemoveUserdefinedCommand(command.Name);
		}

		void _dllWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
		{
			ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery> newCommand = Reflection.ReflectionUtil.LoadTypeOfInterfaceFromAssembly<ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>>(e.FullPath);
			newCommand.Location = e.FullPath;

			CommandManager.InsertUserdefinedCommand(newCommand);
		}

		void _dllWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
		{
			//TODO: implement this
		}

		void _dllWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
		{
			//TODO: implement this
		}
	}
}
