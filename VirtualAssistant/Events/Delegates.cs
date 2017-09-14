using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistant
{
    public delegate void ConsoleWriteEventHandler(object sender, ConsoleWriteEventArgs e);
    public delegate void ApplicationClose(object sender, EventArgs e);
}
