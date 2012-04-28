using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IRCSharp.Kernel.Watchers
{
    /// <summary>
    /// Sets up a directory filesystem watcher that watches for changes made to dll files.
    /// This is not thread safe.
    /// </summary>
    public sealed class DllFileWatcher
    {
        public event FileSystemEventHandler Changed;
        public event FileSystemEventHandler Created;
        public event FileSystemEventHandler Deleted;
        public event RenamedEventHandler Renamed;

        private string _filePath;
        private System.IO.FileSystemWatcher _watcher = null;

        /// <summary>
        /// Constructor. Hooking up events.
        /// </summary>
        /// <param name="directoryPath">The directory to watch.</param>
        public DllFileWatcher(string directoryPath)
        {
            _filePath = directoryPath;
            _watcher = new System.IO.FileSystemWatcher(_filePath, "*.dll");
            _watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
            _watcher.Created += new System.IO.FileSystemEventHandler(watcher_Created);
            _watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Deleted);
            _watcher.Renamed += new System.IO.RenamedEventHandler(watcher_Renamed);
            _watcher.NotifyFilter = System.IO.NotifyFilters.LastAccess | System.IO.NotifyFilters.LastWrite
           | System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName;

        }

        /// <summary>
        /// Start watching directory.
        /// </summary>
        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stop watching directory.
        /// </summary>
        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }

        void watcher_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            if (Changed != null)
            {
                RenamedEventHandler handler = Renamed;
                handler(sender, e);
            }
        }

        void watcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
        {
            if (Deleted != null)
            {
                FileSystemEventHandler handler = Deleted;
                handler(sender, e);
            }
        }

        void watcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            if (Created != null)
            {
                FileSystemEventHandler handler = Created;
                handler(sender, e);
            }
        }

        void watcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            if (Changed != null)
            {
                FileSystemEventHandler handler = Changed;
                handler(sender, e);
            }
        }
    }
}
