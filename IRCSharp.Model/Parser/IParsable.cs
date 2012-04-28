using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model.Parser
{
    internal interface IParsable
    {
        ParserStatus Parse();
    }
}
