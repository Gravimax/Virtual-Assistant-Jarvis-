using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistant.Models;

namespace VirtualAssistant.CommandProcessing.Commands
{
    public class ConditionCommand : ICommandInstance
    {
        public ReturnResult RunCommand(string commandLine)
        {
            if (commandLine.Contains("hello"))
            {
                return new ReturnResult { Response = "Hello" };
            }
            else if (commandLine.Contains("how are you"))
            {
                return new ReturnResult { Response = "I'm doing well" };
            }
            else if (commandLine.Contains("status"))
            {
                return new ReturnResult { Response = "There are currently " + EventLogging.EventLogger.GetExceptionCount() + " new exceptions recorded" };
            }
            else if (commandLine.Contains("clear exceptions"))
            {
                EventLogging.EventLogger.ResetExceptionCount();
                return new ReturnResult { Response = "The exception log count has been reset" };
            }
            else if (commandLine.Contains("thank you"))
            {
                return new ReturnResult { Response = "your welcome" };
            }
            else
            {
                return new ReturnResult { Response = "Hello" };
            }
        }
    }
}
