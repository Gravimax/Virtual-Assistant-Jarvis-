using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistant.Models.Weather
{
    public class Condition
    {
        public string Code { get; set; }

        public string Date { get; set; }

        public string Temp { get; set; }

        public string Text { get; set; }
    }
}
