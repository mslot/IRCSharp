using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model
{
    public interface ICommand : IIdentifiable<string>
    {
        string Location { get; set; }
        void Execute(object sender, params string[] arguments);
    }
}
