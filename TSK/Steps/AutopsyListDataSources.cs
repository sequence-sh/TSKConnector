using Reductech.EDR.Core.Internal.Errors;

namespace Reductech.EDR.Connectors.TSK.Steps;

/// <summary>
/// List all Data Sources in an Autopsy Case.
/// The result is written to the 'Command Output' folder in the case
/// </summary>
public sealed class AutopsyListDataSources : ExistingCaseStep
{
    /// <inheritdoc />
    public override IStepFactory StepFactory =>
        new SimpleStepFactory<AutopsyListDataSources, Unit>();

    /// <inheritdoc />
    public override Task<Result<IReadOnlyList<string>, IError>> GetArguments(
        IStateMonad stateMonad,
        CancellationToken cancellationToken)
    {
        return CaseDirectory
            .WrapStringStream()
            .Run(stateMonad, cancellationToken)
            .Map(
                caseDirectory => new List<string>()
                    {
                        "--nosplash", "--caseDir", $"{caseDirectory}", "--listAllDataSources"
                    } as
                    IReadOnlyList<string>
            );
    }
}
