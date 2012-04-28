using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model
{
    public interface IIdentifiable<T>
    {
        T Name { get; }
    }
}
