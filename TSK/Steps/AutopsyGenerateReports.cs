using Reductech.Sequence.Core.Internal.Errors;

namespace Reductech.Sequence.Connectors.TSK.Steps;

/// <summary>
/// Generate Reports
/// </summary>
public sealed class AutopsyGenerateReports : ExistingCaseStep
{
    /// <inheritdoc />
    public override IStepFactory StepFactory { get; } =
        new SimpleStepFactory<AutopsyGenerateReports, Unit>();

    /// <summary>
    /// The Report Profile to use to generate the report.
    /// You can create new Report Profiles in the Autopsy User Interface 
    /// </summary>
    [StepProperty(2)]
    [DefaultValueExplanation("Use the Default Profile")]
    public IStep<StringStream>? ProfileName { get; set; } = null;

    /// <inheritdoc />
    public override ValueTask<Result<IReadOnlyList<string>, IError>> GetArguments(
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

                        var list = new List<string>
                        {
                            "--nosplash", "--caseDir", $"{caseDirectory}",
                        };

                        if (profileName.HasValue)
                            list.Add($"--generateReports={profileName.Value}");
                        else
                        {
                            list.Add("--generateReports");
                        }

                        return list as IReadOnlyList<string>;
                    }
                )
            ;
    }
}
