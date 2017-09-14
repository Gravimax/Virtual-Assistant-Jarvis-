using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistant
{
    public enum AppendNameType
    {
        Yes,
        No,
        Default
    }

    public enum ReturnDisplayType
    {
        HTML,
        Text,
        None
    }

    public enum VirtualCommandType
    {
        Normal,
        ShellCommand,
        InternalCommand,
        EventCommand
    }
}
