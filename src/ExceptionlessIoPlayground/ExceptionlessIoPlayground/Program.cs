using H.Necessaire;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionlessIoPlayground
{
    class Program
    {
        static int Main(string[] args)
        {
            OperationResult<ExitCode> executionResult = MainAsync(args).GetAwaiter().GetResult();

            return (int)executionResult.Payload;
        }

        static async Task<OperationResult<ExitCode>> MainAsync(params string[] args)
        {
            OperationResult<ExitCode> result = OperationResult.Win().WithPayload(ExitCode.Success);

            await
                new Func<Task>(async () =>
                {
                    result = await new ExceptionlessIoScenariosRunner(Cfg.ExceptionlessApiKey).Run(args);
                })
                .TryOrFailWithGrace(
                    onFail: x => result = OperationResult.Fail(x).WithPayload(ExitCode.Exception)
                );

            await ReportExecutionEnd(result);

            return result;
        }

        private static async Task ReportExecutionEnd(OperationResult<ExitCode> executionResult)
        {
            await Printer.PrintMessage(isTimestampped: false);
            await Printer.PrintMessage($"DONE execution with exit code [{(int)executionResult.Payload}:{executionResult.Payload}]");
            string[] reasons = executionResult.FlattenReasons() ?? new string[0];
            if (reasons.Any())
            {
                await Printer.PrintMessage(isTimestampped: false);
                await Printer.PrintMessage($"Reason(s):{Environment.NewLine}=========={Environment.NewLine}", isTimestampped: false);
                await Printer.PrintMessage(string.Join($"{Environment.NewLine}{Environment.NewLine}", reasons), isTimestampped: false);
            }
            Console.ReadLine();
        }
    }
}
