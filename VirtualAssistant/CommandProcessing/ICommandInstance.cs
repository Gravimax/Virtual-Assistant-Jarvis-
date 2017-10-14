using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistant.Models;

namespace VirtualAssistant.CommandProcessing
{
    public interface ICommandInstance
    {
        ReturnResult RunCommand(string commandLine);
    }
}
