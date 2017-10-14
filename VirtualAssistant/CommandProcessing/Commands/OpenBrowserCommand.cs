using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistant.Models;

namespace VirtualAssistant.CommandProcessing.Commands
{
    public class OpenBrowserCommand : ICommandInstance
    {
        public OpenBrowserCommand()
        {

        }


        public ReturnResult RunCommand(string commandLine)
        {
            System.Diagnostics.Process.Start("https://www.google.com");
            return new ReturnResult { Response = string.Empty };
        }
    }
}
