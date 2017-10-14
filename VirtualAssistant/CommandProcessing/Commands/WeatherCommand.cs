using System;
using System.Collections.Generic;
using System.Linq;
using VirtualAssistant.Models;
using YahooWeather.Models;

namespace VirtualAssistant.CommandProcessing.Commands
{
    public class WeatherCommand : ICommandInstance
    {
        private List<string> dayList = new List<string> { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };


        public WeatherCommand()
        {

        }


        public ReturnResult RunCommand(string commandLine)
        {
            ReturnResult result = null;

            if (commandLine.Contains("current"))
            {
                result = GetCurrentWeather();
            }
            else if (commandLine.Contains("tomorrow"))
            {
                DateTime tomorrow = DateTime.Now.AddDays(1);
                string day = tomorrow.DayOfWeek.ToString().ToLower();
                result = GetForcastForDay(day);
            }
            else
            {
                string[] temp = commandLine.Split(' ');
                List<string> words = temp.Where(word => dayList.Any(x => word.Contains(x))).ToList();

                if (words.Count > 0)
                {
                    result = GetForcastForDay(words.First());
                }
                else
                {
                    result = GetCurrentWeather();
                }
            }

            return result;
        }


        private ReturnResult GetCurrentWeather()
        {
            Weather weather = GetTheWeather("Dana Point, Ca"); // Eventually add location to config file

            return new ReturnResult { Response = "The temperature is currently " + weather.Condition.Temp + " degrees", Display = weather.Description, DisplayType = ReturnDisplayType.HTML };
        }


        private ReturnResult GetForcastForDay(string day)
        {
            Weather weather = GetTheWeather("Dana Point, Ca");

            foreach (var item in weather.Forcasts) // Convert from short to full name
            {
                item.Day = Utilities.ConvertDay(item.Day);
            }

            Forcast forcast = weather.Forcasts.FirstOrDefault(x => x.Day == day);

            if (forcast != null)
            {
                return new ReturnResult { Response = day + "s forecast is " + forcast.Text + " with a high of " + forcast.High + " and a low of " + forcast.Low, Display = weather.Description, DisplayType = ReturnDisplayType.HTML };
            }
            else
            {
                return new ReturnResult { Response = "I'm sorry but, " + day + "s forcast is not currently available" };
            }
        }


        private Weather GetTheWeather(string location)
        {
            YahooWeather.GetWeather getWeather = new YahooWeather.GetWeather();
            return getWeather.CurrentWeather(location);
        }
    }
}
