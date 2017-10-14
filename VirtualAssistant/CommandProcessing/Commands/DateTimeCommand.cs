using System;
using System.Collections.Generic;
using VirtualAssistant.Models;

namespace VirtualAssistant.CommandProcessing.Commands
{
    public class DateTimeCommand : ICommandInstance
    {
        private List<string> dayList = new List<string> { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };


        public DateTimeCommand()
        {

        }


        public ReturnResult RunCommand(string commandLine)
        {
            DateTime now = DateTime.Now;

            if (commandLine.Contains("tomorrow"))
            {
                now = now.AddDays(1);
                return new ReturnResult { Response = "Tomorrows date is " + now.ToString("dddd MMMM dd") };
            }
            else if (commandLine.Contains("date"))
            {
                if (CheckDay(commandLine))
                {
                    string day = GetDay(commandLine);
                    DateTime future = GetFutureDate(day);
                    return new ReturnResult { Response = day + "s date is " + future.ToString("dddd MMMM dd") };
                }
                else
                {
                    return new ReturnResult { Response = "The current date is " + now.ToString("dddd MMMM dd") };
                }

            }
            else if (commandLine.Contains("time"))
            {
                return new ReturnResult { Response = "The current time is " + now.ToString("h mm tt") };
            }
            else
            {
                return new ReturnResult { Response = "I'm not sure what day you want" };
            }
        }


        private bool CheckDay(string commandLine)
        {
            foreach (var item in dayList)
            {
                if (commandLine.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }


        private string GetDay(string commandLine)
        {
            foreach (var item in dayList)
            {
                if (commandLine.Contains(item))
                {
                    return item;
                }
            }

            return DateTime.Now.DayOfWeek.ToString().ToLower();
        }


        private DateTime GetFutureDate(string day)
        {
            if (!string.IsNullOrEmpty(day))
            {
                for (int i = 1; i < 8; i++)
                {
                    DateTime dateTime = DateTime.Now.AddDays(i);
                    if (dateTime.DayOfWeek.ToString().ToLower() == day)
                    {
                        return dateTime;
                    }
                }
            }

            return DateTime.Now;
        }
    }
}
