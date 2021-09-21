using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Reductech.EDR.Core;
using Reductech.EDR.Core.Attributes;
using Reductech.EDR.Core.Internal;
using Reductech.EDR.Core.Internal.Errors;
using Reductech.EDR.Core.Util;

namespace Reductech.EDR.Connectors.TSK.Steps
{

/// <summary>
/// Generate Reports
/// </summary>
public sealed class AutopsyGenerateReports : ExistingCaseStep
{
    /// <inheritdoc />
    public override IStepFactory StepFactory { get; } =
        new SimpleStepFactory<AutopsyGenerateReports, Unit>();

    /// <summary>
    /// The Report Profile to use to generate the report
    /// </summary>
    [StepProperty(2)]
    [DefaultValueExplanation("Use the Default Profile")]
    public IStep<StringStream>? ProfileName { get; set; } = null;

    /// <inheritdoc />
    public override Task<Result<IReadOnlyList<string>, IError>> GetArguments(
        IStateMonad stateMonad,
        CancellationToken cancellationToken)
    {
        return
            stateMonad.RunStepsAsync(
                    CaseDirectory.WrapStringStream(),
                    ProfileName.WrapNullable(StepMaps.String()),
                    cancellationToken
                )
                .Map(
                    x =>
                    {
                        var (caseDirectory, profileName) = x;

                        var list = new List<string> { $"{caseDirectory}", "--generateReports" };

                        if (profileName.HasValue)
                            list.Add(profileName.Value);

                        return list as IReadOnlyList<string>;
                    }
                )
            ;
    }
}

}
