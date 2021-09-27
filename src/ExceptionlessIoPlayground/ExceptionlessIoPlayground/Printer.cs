using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace ExceptionlessIoPlayground
{
    static class Printer
    {
        public static Task PrintMessage(string message = null, bool isTimestampped = true)
        {
            if (isTimestampped)
                Console.WriteLine($"{DateTime.Now} - {message}");
            else
                Console.WriteLine(message);

            return true.AsTask();
        }

        public static Task PrintProgress()
        {
            Console.Write(".");
            return true.AsTask();
        }
    }
}
