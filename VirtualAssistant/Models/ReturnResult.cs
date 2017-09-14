using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistant.Models
{
    public class ReturnResult
    {
        public CommandItem Command { get; set; }

        public string Response { get; set; }

        public string Display { get; set; }

        public ReturnDisplayType DisplayType = ReturnDisplayType.None;

        public bool HasDisplay
        {
            get { return !string.IsNullOrEmpty(Display); }            
        }
    }
}
