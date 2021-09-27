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

            ReportExecutionEnd(executionResult);

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

            return result;
        }

        private static void ReportExecutionEnd(OperationResult<ExitCode> executionResult)
        {
            Console.WriteLine($"{Environment.NewLine}{DateTime.Now} - DONE execution with exit code [{(int)executionResult.Payload}:{executionResult.Payload}]");
            string[] reasons = executionResult.FlattenReasons() ?? new string[0];
            if (reasons.Any())
            {
                Console.WriteLine($"{Environment.NewLine}Reason(s):{Environment.NewLine}=========={Environment.NewLine}");
                Console.WriteLine(string.Join($"{Environment.NewLine}{Environment.NewLine}", reasons));
            }
            Console.ReadLine();
        }
    }
}
