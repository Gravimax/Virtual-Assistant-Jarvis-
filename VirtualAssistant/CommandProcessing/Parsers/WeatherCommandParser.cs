using System;
using System.Collections.Generic;
using VirtualAssistant.CommandProcessing;
using VirtualAssistant.CommandProcessing.Commands;

namespace VirtualAssistant.CommandProcessing.Parsers
{
    public class WeatherCommandParser : ICommand
    {
        public List<string> CommandList { get; private set; }


        public WeatherCommandParser()
        {
            CommandList = new List<string> { "weather", "forcast", "current", "tomorrow", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
        }


        public ICommandInstance GetCommandInstance()
        {
            return new WeatherCommand();
        }
    }
}
