using System.Collections.Generic;
using VirtualAssistant.CommandProcessing.Commands;

namespace VirtualAssistant.CommandProcessing.Parsers
{
    public class CloseApplicationCommandParser : ICommand
    {
        public List<string> CommandList { get; private set; }


        public CloseApplicationCommandParser()
        {
            CommandList = new List<string> { "close", "quit", "exit", "application", "program" };
        }


        public ICommandInstance GetCommandInstance()
        {
            return new CloseApplicationCommand();
        }
    }
}
