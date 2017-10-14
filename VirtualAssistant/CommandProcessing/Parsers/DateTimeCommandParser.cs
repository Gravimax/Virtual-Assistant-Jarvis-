using System.Collections.Generic;
using VirtualAssistant.CommandProcessing.Commands;

namespace VirtualAssistant.CommandProcessing.Parsers
{
    public class DateTimeCommandParser : ICommand
    {
        public List<string> CommandList { get; private set; }


        public DateTimeCommandParser()
        {
            CommandList = new List<string> { "date", "time", "day", "tomorrow", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
        }


        public ICommandInstance GetCommandInstance()
        {
            return new DateTimeCommand();
        }
    }
}
