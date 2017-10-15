using System;
using System.IO;

namespace VirtualAssistant.EventLogging
{
    public class EventLogger
    {
        private static int exceptionCount = 0;

        private static object locker = new object();


        public static void WriteEventLog(string message, Exception ex = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                lock (locker)
                {
                    try
                    {
                        using (StreamWriter sw = File.AppendText(Utilities.GetEventLogPath()))
                        {
                            sw.WriteLine("[" + DateTime.Now + "] " + message);
                        }
                    }
                    catch (Exception)
                    {
                        // ToDo: log error to the windows log as this is a serious exception
                    }
                }
            }
            else if (ex != null)
            {
                lock (locker)
                {
                    try
                    {
                        using (StreamWriter sw = File.AppendText(Utilities.GetExceptionLogPath()))
                        {
                            sw.WriteLine("[" + DateTime.Now + "] " + ex.Message);
                            sw.WriteLine("Stack Trace: " + ex.StackTrace);
                            exceptionCount++;
                        }
                    }
                    catch (Exception)
                    {
                        // ToDo: log error to the windows log as this is a serious exception
                    }
                }
            }
        }


        public static int GetExceptionCount()
        {
            return exceptionCount;
        }


        public static void ResetExceptionCount()
        {
            exceptionCount = 0;
        }
    }
}
