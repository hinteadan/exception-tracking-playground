using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace ExceptionlessIoPlayground
{
    class Program
    {
        static int Main(string[] args)
        {
            ExitCode exitCode = MainAsync(args).GetAwaiter().GetResult();

            Console.WriteLine($"Done with [{(int)exitCode}:{exitCode}] @ {DateTime.Now}");
            Console.ReadLine();

            return (int)exitCode;
        }

        static async Task<ExitCode> MainAsync(params string[] args)
        {
            return await ExitCode.Success.AsTask();
        }
    }
}
