using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using VirtualAssistant.Models;
using VirtualAssistant.Models.Weather;

namespace VirtualAssistant.InternalCommands
{
    /// <summary>
    /// Parses Yahoo weather rss feed to get the current and forcast weather.
    /// </summary>
    /// <seealso cref="VirtualAssistant.InternalCommands.IInternalCommand" />
    public class GetWeather : IInternalCommand
    {
        private string CONDITION = "<yweather:condition";
        private string FORCAST = "<yweather:forecast";
        private string WIND = "<yweather:wind";
        private string ATMOSPHERE = "<yweather:atmosphere";
        private string ASTRONOMY = "<yweather:astronomy";
        private string START_DESC = "<description>";
        private string END_DESC = "</description>";
        private string CDATA_START = "[CDATA[";
        private string CDATA_END = "]]&gt;";

        public ReturnResult RunCommand(CommandItem commandItem, string command)
        {
            try
            {
                switch (command)
                {
                    case "how's the weather":
                    case "what's the weather like":
                    case "what's it like outside":
                        return CurrentWeather(commandItem);

                    case "what will tomorrow be like":
                    case "what's tomorrows forecast":
                    case "what's tomorrow like":
                        return GetTommorrow(commandItem);

                    case "what's the temperature":
                    case "what's the temperature outside":
                        return GetTemp(commandItem);

                    default:
                        return new ReturnResult { Response = "There was an error getting the weather. Just check out side" };
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return new ReturnResult { Response = "There was an error getting the weather. Just check out side" };
            }
        }


        // https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text=%22nome%2C%20ak%22)&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys
        // https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text="nome, ak")&format=xml&env=store://datatables.org/alltableswithkeys


        private ReturnResult GetTemp(CommandItem command)
        {
            YahooWeather weather = GetCurrentWeather(command);
            return new ReturnResult { Response = "The temperature is currently " + weather.Condition.Temp + " degrees", Display = weather.Description, DisplayType = ReturnDisplayType.HTML };
        }


        private ReturnResult GetTommorrow(CommandItem command)
        {
            YahooWeather weather = GetCurrentWeather(command);
            Forcast forcast = weather.Forcasts.FirstOrDefault(x => x.Date.Date == DateTime.Now.AddDays(1).Date);

            if (forcast != null)
            {
                return new ReturnResult { Response = "Tomorrows forecast is " + forcast.Text + " with a high of " + forcast.High + " and a low of " + forcast.Low, Display = weather.Description, DisplayType = ReturnDisplayType.HTML };
            }
            else
            {
                return new ReturnResult { Response = "I'm sorry but, tomorrows forcast is not currently available" };
            }
        }


        private ReturnResult CurrentWeather(CommandItem command)
        {
            YahooWeather weather = GetCurrentWeather(command);
            return new ReturnResult { Response = "The weather is " + weather.Condition.Text + " at " + weather.Condition.Temp + " degrees. With a humidity of " + weather.Atmosphere.Humidity + " and a windspeed of " + weather.Wind.Speed + " miles per hour", Display = weather.Description, DisplayType = ReturnDisplayType.HTML };
        }


        private YahooWeather GetCurrentWeather(CommandItem command)
        {
            string[] cmdArgs = command.CommandArgs.Split('/');

            string query = String.Format("https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='{0}')&format=xml&env=store://datatables.org/alltableswithkeys", cmdArgs[1]);
            XDocument xDoc = XDocument.Load(query);
            string data = xDoc.ToString();
            data = data.Replace("\r\n", "");

            YahooWeather weather = new YahooWeather();
            weather.Condition = ParseCondition(data);
            weather.Wind = ParseWind(data);
            weather.Atmosphere = ParseAtmosphere(data);
            weather.Astronomy = ParseAstronomy(data);
            weather.Forcasts = ParseForcast(data);
            weather.Description = ParseDescription(data);

            return weather;
        }


        #region Parse Data


        private Condition ParseCondition(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                int startIndex = GetElementIndex(0, CONDITION, data) + CONDITION.Length;
                int length = GetElementIndex(startIndex, " />", data) - startIndex;

                data = data.Substring(startIndex, length).Trim();

                Condition cond = new Condition();

                cond.Code = ParseAttribute("code", data);
                cond.Date = ParseAttribute("date", data);
                cond.Temp = ParseAttribute("temp", data);
                cond.Text = ParseAttribute("text", data);

                return cond;
            }

            return null;
        }


        public Atmosphere ParseAtmosphere(string data)
        {
            int startIndex = GetElementIndex(0, ATMOSPHERE, data) + ATMOSPHERE.Length;
            int length = GetElementIndex(startIndex, " />", data) - startIndex;

            data = data.Substring(startIndex, length).Trim();

            Atmosphere atmos = new Atmosphere();

            atmos.Humidity = ParseAttribute("humidity", data);
            atmos.Pressure = ParseAttribute("pressure", data);
            atmos.Rising = ParseAttribute("rising", data);
            atmos.Visibility = ParseAttribute("visibility", data);

            return atmos;
        }


        public Astronomy ParseAstronomy(string data)
        {
            int startIndex = GetElementIndex(0, ASTRONOMY, data) + ASTRONOMY.Length;
            int length = GetElementIndex(startIndex, " />", data) - startIndex;

            data = data.Substring(startIndex, length).Trim();

            Astronomy astro = new Astronomy();

            astro.Sunrise = ParseAttribute("sunrise", data);
            astro.Sunset = ParseAttribute("sunset", data);

            return astro;
        }


        public Wind ParseWind(string data)
        {
            int startIndex = GetElementIndex(0, WIND, data) + WIND.Length;
            int length = GetElementIndex(startIndex, " />", data) - startIndex;

            data = data.Substring(startIndex, length).Trim();

            Wind wind = new Wind();

            wind.Chill = ParseAttribute("chill", data);
            wind.Direction = ParseAttribute("direction", data);
            wind.Speed = ParseAttribute("speed", data);

            return wind;
        }


        public List<Forcast> ParseForcast(string data)
        {
            int startIndex = GetElementIndex(0, FORCAST, data) + FORCAST.Length;
            int endIndex = GetElementIndex(startIndex, " />", data);
            int length = endIndex - startIndex;

            List<Forcast> temp = new List<Forcast>();

            while (startIndex > -1)
            {
                string dataTemp = data.Substring(startIndex, length).Trim();

                temp.Add(new Forcast
                {
                    Code = ParseAttribute("code", dataTemp),
                    Date = DateTime.Parse(ParseAttribute("date", dataTemp)),
                    Day = ParseAttribute("day", dataTemp),
                    High = ParseAttribute("high", dataTemp),
                    Low = ParseAttribute("low", dataTemp),
                    Text = ParseAttribute("text", dataTemp),
                });

                startIndex = GetElementIndex(endIndex, FORCAST, data);
                if (startIndex > -1)
                {
                    endIndex = GetElementIndex(startIndex, " />", data);
                    length = endIndex - startIndex;
                }
            };

            return temp;
        }


        public string ParseDescription(string data)
        {
            int startIndex = GetElementIndex(0, "<item>", data) + START_DESC.Length;
            startIndex = GetElementIndex(startIndex, START_DESC, data);
            int endIndex = GetElementIndex(startIndex, END_DESC, data);

            data = data.Substring(startIndex, endIndex - startIndex).Trim();

            int cdataStart = data.IndexOf(CDATA_START) + CDATA_START.Length;
            int cdataEnd = data.IndexOf(CDATA_END);
            int length = cdataEnd - cdataStart;

            data = data.Substring(cdataStart, length);
            return HttpUtility.HtmlDecode(data);
        }


        public string ParseAttribute(string element, string data)
        {
            int index = data.IndexOf(element) + element.Length + 2;
            int endIndex = GetElementIndex(index, data, '"');
            return data.Substring(index, endIndex - index);
        }


        private int GetElementIndex(int startIndex, string element, string data)
        {
            return data.IndexOf(element, startIndex);
        }


        private int GetElementIndex(int startIndex, string data, char element)
        {
            return data.IndexOf(element, startIndex);
        }


        #endregion
    }
}
