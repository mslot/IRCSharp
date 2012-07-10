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
				if (Reflection.ReflectionUtil.IsOfType<ICommand<string, IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery>>(absoluteFilePath))
				{
					commandType = Reflection.ReflectionUtil.GetTypeOf<ICommand<string, IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery>>(absoluteFilePath);
					string name = Reflection.ReflectionUtil.GetUserdefinedName(directoryPath);

					CommandManager.InsertUserdefinedCommand(commandType, name, absoluteFilePath);
				}
				else if (Reflection.ReflectionUtil.IsOfType<ICommand<Model.Query.ResponseCommand, IRCSharp.Kernel.Model.Query.IRCCommandQuery>>(absoluteFilePath))
				{
					commandType = Reflection.ReflectionUtil.GetTypeOf<ICommand<Model.Query.ResponseCommand, IRCSharp.Kernel.Model.Query.IRCCommandQuery>>(absoluteFilePath);
					Model.Query.ResponseCommand name = Reflection.ReflectionUtil.GetIRCCommandName(absoluteFilePath);

					CommandManager.InsertIRCCommand(commandType, name, absoluteFilePath);
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
			string absoluteFilePath = e.FullPath;
			if (Reflection.ReflectionUtil.IsOfType<ICommand<string, IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery>>(absoluteFilePath))
			{
				Type commandType = Reflection.ReflectionUtil.GetTypeOf<ICommand<string, IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery>>(absoluteFilePath);
				string name = Reflection.ReflectionUtil.GetUserdefinedName(absoluteFilePath);

				CommandManager.InsertUserdefinedCommand(commandType, name, absoluteFilePath);
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
