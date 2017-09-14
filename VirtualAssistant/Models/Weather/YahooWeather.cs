using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistant.Models.Weather
{
    public class YahooWeather
    {
        public Astronomy Astronomy { get; set; }

        public Atmosphere Atmosphere { get; set; }

        public Wind Wind { get; set; }

        public Condition Condition { get; set; }

        public List<Forcast> Forcasts { get; set; }

        public string Description { get; set; }
    }
}
