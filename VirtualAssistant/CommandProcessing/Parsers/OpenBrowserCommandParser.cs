using System.Collections.Generic;
using VirtualAssistant.CommandProcessing.Commands;

namespace VirtualAssistant.CommandProcessing.Parsers
{
    public class OpenBrowserCommandParser : ICommand
    {
        public List<string> CommandList { get; private set; }


        public OpenBrowserCommandParser()
        {
            CommandList = new List<string> { "browser", "open" };
        }


        public ICommandInstance GetCommandInstance()
        {
            return new OpenBrowserCommand();
        }
    }
}
