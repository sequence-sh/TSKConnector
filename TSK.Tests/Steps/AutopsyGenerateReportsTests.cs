﻿using System.Linq;
using System.Text;
using System.Threading;
using CSharpFunctionalExtensions;
using Moq;
using Sequence.Connectors.TSK.Steps;
using Sequence.Core.ExternalProcesses;

namespace Sequence.Connectors.TSK.Tests.Steps;

public partial class AutopsyGenerateReportsTests : StepTestBase<AutopsyGenerateReports, Unit>
{
    /// <inheritdoc />
    protected override IEnumerable<StepCase> StepCases
    {
        get
        {
            yield return new StepCase(
                        "Generate Reports with  profile",
                        new AutopsyGenerateReports()
                        {
                            CaseDirectory = StaticHelpers.Constant("TestCaseDirectory"),
                            ProfileName   = StaticHelpers.Constant("TestReportProfile"),
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
                                            "--generateReports=TestReportProfile"
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
        base.ErrorCases.Select(x => x.WithTestTSKSettings<ErrorCase>());
}
