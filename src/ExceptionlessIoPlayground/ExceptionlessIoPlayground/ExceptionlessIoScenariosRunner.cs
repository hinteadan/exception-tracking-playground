using Exceptionless;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace ExceptionlessIoPlayground
{
    class ExceptionlessIoScenariosRunner
    {
        #region Construct
        readonly ExceptionlessClient exceptionlessClient;
        public ExceptionlessIoScenariosRunner(string exceptionlessApiKey)
        {
            this.exceptionlessClient = new Exceptionless.ExceptionlessClient(exceptionlessApiKey);
        }
        #endregion

        public async Task<OperationResult<ExitCode>> Run(params string[] args)
        {
            using (new TimeMeasurement(x => Printer.PrintMessage($"Done running Exceptionless.io scenarios after {x.TotalSeconds} secs.").Wait()))
            {
                OperationResult<ExitCode> result = OperationResult.Win().WithPayload(ExitCode.Success);

                await
                    new Func<Task>(async () =>
                    {
                        await Printer.PrintMessage("Running Exceptionless.io scenarios...");

                        for (int i = 0; i < 10; i++)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(.5));
                            await Printer.PrintProgress();
                        }

                        Console.WriteLine();

                        throw new NotImplementedException("ExceptionlessIoScenariosRunner not yet implemented");
                    })
                    .TryOrFailWithGrace(
                        onFail: x => result = OperationResult.Fail(x).WithPayload(ExitCode.Exception)
                    );

                return result;
            }
        }
    }
}
