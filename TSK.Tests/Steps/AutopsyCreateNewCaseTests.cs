using System.Linq;
using System.Text;
using System.Threading;
using CSharpFunctionalExtensions;
using Moq;
using Reductech.Sequence.Connectors.TSK.Steps;
using Reductech.Sequence.Core.ExternalProcesses;

namespace Reductech.Sequence.Connectors.TSK.Tests.Steps;

public partial class AutopsyCreateNewCaseTests : StepTestBase<AutopsyCreateNewCase, Unit>
{
    /// <inheritdoc />
    protected override IEnumerable<StepCase> StepCases
    {
        get
        {
            yield return new StepCase(
                        "Create Case with CaseType",
                        new AutopsyCreateNewCase()
                        {
                            CaseName          = Constant("My New Case"),
                            CaseBaseDirectory = Constant("Case Base Directory"),
                            CaseType          = Constant(AutopsyCaseType.single)
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
                                            "--nosplash", "--createCase", "--caseName",
                                            "My New Case", "--caseBaseDir",
                                            "Case Base Directory", "--caseType=single"
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
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<ErrorCase> ErrorCases =>
        base.ErrorCases.Select(x => x.WithTestTSKSettings());
}
