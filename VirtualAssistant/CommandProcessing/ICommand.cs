using System.Collections.Generic;

namespace VirtualAssistant.CommandProcessing
{
    public interface ICommand
    {
        List<string> CommandList { get; }

        ICommandInstance GetCommandInstance();
    }
}
