using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistant.Models;

namespace VirtualAssistant.CommandProcessing.Commands
{
    public class UnknownCommand : ICommandInstance
    {
        public List<string> CommandList { get; private set; }

        private string command;

        public UnknownCommand(string command)
        {
            this.command = command;
        }


        public ReturnResult RunCommand(string commandLine)
        {
            return new ReturnResult { Response = command };
        }
    }
}
