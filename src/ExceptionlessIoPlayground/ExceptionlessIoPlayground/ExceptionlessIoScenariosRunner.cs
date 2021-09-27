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
            using (new TimeMeasurement(x => Console.WriteLine($"{DateTime.Now} - Done running Exceptionless.io scenarios after {x.TotalSeconds} secs.")))
            {
                OperationResult<ExitCode> result = OperationResult.Win().WithPayload(ExitCode.Success);

                await
                    new Func<Task>(async () =>
                    {
                        Console.WriteLine($"{DateTime.Now} - Running Exceptionless.io scenarios...");

                        for (int i = 0; i < 10; i++)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(.5));
                            Console.Write(".");
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
