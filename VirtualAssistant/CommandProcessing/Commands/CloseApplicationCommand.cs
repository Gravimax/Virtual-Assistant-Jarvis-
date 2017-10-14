using VirtualAssistant.Models;

namespace VirtualAssistant.CommandProcessing.Commands
{
    public class CloseApplicationCommand : ICommandInstance
    {
        public CloseApplicationCommand()
        {

        }


        public ReturnResult RunCommand(string commandLine)
        {
            return new ReturnResult { Response = "Good bye" };
        }
    }
}
