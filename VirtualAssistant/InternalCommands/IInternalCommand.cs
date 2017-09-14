using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistant.Models;

namespace VirtualAssistant.InternalCommands
{
    public interface IInternalCommand
    {
        ReturnResult RunCommand(CommandItem commandItem, string command);
    }
}
