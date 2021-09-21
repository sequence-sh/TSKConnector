using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using CSharpFunctionalExtensions;
using Moq;
using Reductech.EDR.Connectors.TSK.Steps;
using Reductech.EDR.Core;
using Reductech.EDR.Core.ExternalProcesses;
using Reductech.EDR.Core.Internal;
using Reductech.EDR.Core.Internal.Errors;
using Reductech.EDR.Core.TestHarness;
using Entity = Reductech.EDR.Core.Entity;

namespace Reductech.EDR.Connectors.TSK.Tests.Steps
{

public partial class TSKListDataSourcesTests : StepTestBase<AutopsyListDataSources, Array<Entity>>
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
                        new List<Entity>() { Entity.Create(("Data", "My Line")) }.ToSCLArray()
                    )
                    .WithTestTSKSettings()
                    .WithExternalProcessAction(
                        x => x.Setup(
                                runner => runner
                                    .StartExternalProcess(
                                        "C:/AutopsyTest",
                                        new[] { "TestCaseDirectory", "--listAllDataSources" },
                                        It.IsAny<IReadOnlyDictionary<string, string>>(),
                                        Encoding.UTF8,
                                        It.IsAny<IStateMonad>(),
                                        It.IsAny<IStep>()
                                    )
                            )
                            .Returns(
                                Result.Success<IExternalProcessReference, IErrorBuilder>(
                                    eprMock.Object
                                )
                            )
                    )
                ;
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<ErrorCase> ErrorCases =>
        base.ErrorCases.Select(x => TestHelpers.WithTestTSKSettings<ErrorCase>(x));
}

}
