using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model
{
    public abstract class CommandBase : ICommand
    {
        public abstract void Execute(object sender, params string[] arguments);
        public abstract string Name { get; }
        public string Location { get; set; }

        public CommandBase()
        {

        }
    }
}
