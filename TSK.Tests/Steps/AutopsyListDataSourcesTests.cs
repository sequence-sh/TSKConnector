using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using CSharpFunctionalExtensions;
using Moq;
using Reductech.EDR.Connectors.TSK.Steps;
using Reductech.EDR.Core.ExternalProcesses;

namespace Reductech.EDR.Connectors.TSK.Tests.Steps;

public partial class
    AutopsyListDataSourcesTests : StepTestBase<AutopsyListDataSources, Unit>
{
    /// <inheritdoc />
    protected override IEnumerable<StepCase> StepCases
    {
        get
        {
            var channel = Channel.CreateUnbounded<(string line, StreamSource source)>();
            channel.Writer.TryWrite(("My Line", StreamSource.Output));
            channel.Writer.Complete();

            var repo    = new MockRepository(MockBehavior.Strict);
            var eprMock = repo.Create<IExternalProcessReference>();

            eprMock.Setup(x => x.OutputChannel)
                .Returns(channel);

            eprMock.Setup(x => x.Dispose());

            yield return new StepCase(
                        "Add Data Source",
                        new AutopsyListDataSources()
                        {
                            CaseDirectory = StaticHelpers.Constant("TestCaseDirectory")
                        },
                        Unit.Default
                    )
                    .WithTestTSKSettings()
                    .WithExternalProcessAction(
                        x => x.Setup(
                                runner => runner
                                    .RunExternalProcess(
                                        "C:/AutopsyTest",
                                        IgnoreNoneErrorHandler.Instance,
                                        new[]
                                        {
                                            "--nosplash", "--caseDir", "TestCaseDirectory",
                                            "--listAllDataSources"
                                        },
                                        It.IsAny<IReadOnlyDictionary<string, string>>(),
                                        Encoding.UTF8,
                                        It.IsAny<IStateMonad>(),
                                        It.IsAny<IStep>(),
                                        It.IsAny<CancellationToken>()
                                    )
                            )
                            .ReturnsAsync(Result.Success<Unit, IErrorBuilder>(Unit.Default))
                    )
                ;
            //    .WithTestTSKSettings()
            //    .WithExternalProcessAction(
            //        x => x.Setup(
            //                runner => runner
            //                    .StartExternalProcess(
            //                        "C:/AutopsyTest",
            //                        new[] { "TestCaseDirectory", "--listAllDataSources" },
            //                        It.IsAny<IReadOnlyDictionary<string, string>>(),
            //                        Encoding.UTF8,
            //                        It.IsAny<IStateMonad>(),
            //                        It.IsAny<IStep>()
            //                    )
            //            )
            //            .Returns(
            //                Result.Success<IExternalProcessReference, IErrorBuilder>(
            //                    eprMock.Object
            //                )
            //            )
            //    )
            //;
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<ErrorCase> ErrorCases =>
        base.ErrorCases.Select(x => x.WithTestTSKSettings<ErrorCase>());
}
