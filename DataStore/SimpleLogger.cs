using System;
using System.Collections.Generic;
using System.Text;

namespace DataStore
{
    class SimpleLogger : ILogger
    {
        public void Log (string message)
        {
            Console.WriteLine(message);
        }

        public void ErrorLog(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*** " + message + "*** ");
            Console.ResetColor();
        }

        public void ErrorLog(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(String.Format("*** Ошибка в методе {0}: {1} *** Трассировка: {2} ***", exception.TargetSite, exception.Message, exception.StackTrace));
            Console.ResetColor();
        }


    }
}
