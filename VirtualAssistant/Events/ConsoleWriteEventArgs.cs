using System;

namespace VirtualAssistant
{
    public class ConsoleWriteEventArgs : EventArgs
    {
        public ConsoleWriteEventArgs(string message, bool isError)
        {
            Message = message;
            IsError = isError;           
        }

        public readonly string Message;

        public readonly bool IsError;
    }
}
