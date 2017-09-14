using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistant.Models;

namespace VirtualAssistant.InternalCommands
{
    public class GetDate : IInternalCommand
    {
        public ReturnResult RunCommand(CommandItem commandItem, string command)
        {
            DateTime now = DateTime.Now;

            switch (command)
            {
                case "what day is it":
                    return new ReturnResult { Response = "The current date is " + now.ToString("dddd MMMM dd") };
                case "what time is it":
                case "what is the time":
                    return new ReturnResult { Response = "The current time is " + now.ToString("h mm tt") };
                default:
                    return new ReturnResult { Response = "The current time is " + now.ToString("h mm tt") };
            }

        }
    }
}
