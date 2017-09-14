using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistant.Models.Weather
{
    public class Forcast
    {
        public string Code { get; set; }

        public DateTime Date { get; set; }

        public string Day { get; set; }

        public string High { get; set; }

        public string Low { get; set; }

        public string Text { get; set; }
    }
}
