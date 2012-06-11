using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Threading.Base
{
	public abstract class Thread
	{
		private System.Threading.Thread _thread = null;
		private string _name = "default name";

		public Thread(string name)
		{
			_name = name;
		}

		public void Start()
		{
			_thread = new System.Threading.Thread(Task);
			_thread.Name = _name;

			if (_thread != null && _thread.ThreadState == System.Threading.ThreadState.Unstarted)
			{
				_thread.Start();
			}
		}

		public void Join()
		{
			_thread.Join();
		}

		public virtual void Abort()
		{
			if (_thread != null)
			{
				if (_thread.ThreadState != System.Threading.ThreadState.Stopped ||
					_thread.ThreadState != System.Threading.ThreadState.Aborted)
				{
					_thread.Abort();
				}
			}
		}

		public virtual void Task()
		{

		}
	}
}
