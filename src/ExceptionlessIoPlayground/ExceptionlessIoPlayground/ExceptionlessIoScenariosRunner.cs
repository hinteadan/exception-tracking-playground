using Bogus;
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
        readonly Faker faker = new Faker();
        public ExceptionlessIoScenariosRunner(string exceptionlessApiKey)
        {
            this.exceptionlessClient = new Exceptionless.ExceptionlessClient(exceptionlessApiKey);
        }
        #endregion

        public async Task<OperationResult<ExitCode>> Run(params string[] args)
        {
            await Printer.PrintMessage("Running Exceptionless.io scenarios...");

            using (new TimeMeasurement(x => Printer.PrintMessage($"Done running Exceptionless.io scenarios after {x.TotalSeconds} secs.").Wait()))
            {
                OperationResult<ExitCode> result = OperationResult.Win().WithPayload(ExitCode.Success);

                await
                    new Func<Task>(async () =>
                    {
                        await Printer.PrintMessage("Submitting a simple LOG...");
                        exceptionlessClient.SubmitLog(faker.Hacker.IngVerb());

                        await Printer.PrintMessage("Submitting a dummy exception...");
                        exceptionlessClient.SubmitException(new InvalidOperationException(faker.Hacker.Verb()));

                        await Printer.PrintMessage("Submitting a dummy event...");
                        exceptionlessClient.SubmitEvent(new Exceptionless.Models.Event
                        {
                            Message = faker.Hacker.IngVerb(),
                            ReferenceId = faker.Random.AlphaNumeric(8),
                            Source = this.GetType().FullName,
                        });

                        await Printer.PrintMessage("Submitting a dummy feature usage...");
                        exceptionlessClient.SubmitFeatureUsage(faker.Commerce.ProductName());
                    })
                    .TryOrFailWithGrace(
                        onFail: x => result = OperationResult.Fail(x).WithPayload(ExitCode.Exception)
                    );

                return result;
            }
        }
    }
}
