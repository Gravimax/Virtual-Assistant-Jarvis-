using System.Collections.Generic;
using VirtualAssistant.CommandProcessing.Commands;

namespace VirtualAssistant.CommandProcessing.Parsers
{
    public class UnknownCommandParser : ICommand
    {
        public List<string> CommandList { get; private set; }

        private string command;

        public UnknownCommandParser(string command)
        {
            this.command = command;
        }


        public ICommandInstance GetCommandInstance()
        {
            return new UnknownCommand(command);
        }
    }
}
