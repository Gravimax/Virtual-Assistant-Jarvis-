using System.Collections.Generic;
using VirtualAssistant.CommandProcessing.Commands;

namespace VirtualAssistant.CommandProcessing.Parsers
{
    public class ConditionParser : ICommand
    {
        public List<string> CommandList { get; private set; }


        public ConditionParser()
        {
            CommandList = new List<string> { "how are you", "hello", "your", "status", "clear exceptions", "thank you" };
        }


        public ICommandInstance GetCommandInstance()
        {
            return new ConditionCommand();
        }
    }
}
