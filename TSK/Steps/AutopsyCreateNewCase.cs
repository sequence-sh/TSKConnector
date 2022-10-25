using Reductech.Sequence.Core.Internal.Errors;

namespace Reductech.Sequence.Connectors.TSK.Steps;

/// <summary>
/// Creates a new Autopsy Case
/// </summary>
public sealed class AutopsyCreateNewCase : AutopsyConsoleStep
{
    /// <inheritdoc />
    public override IStepFactory StepFactory { get; } =
        new SimpleStepFactory<AutopsyCreateNewCase, Unit>();

    /// <summary>
    /// The name of the case to create
    /// </summary>
    [StepProperty(1)]
    [Required]
    public IStep<StringStream> CaseName { get; set; } = null!;

    /// <summary>
    /// The Directory into which to put the case files
    /// </summary>
    [StepProperty(2)]
    [Required]
    public IStep<StringStream> CaseBaseDirectory { get; set; } = null!;

    /// <summary>
    /// The Type of Case to Create
    /// </summary>
    [StepProperty(3)]
    [DefaultValueExplanation("No Case Type Specified")]
    public IStep<SCLEnum<AutopsyCaseType>>? CaseType { get; set; } = null;

    /// <summary>
    /// The Path to the Data Source to add
    /// </summary>
    [StepProperty(4)]
    [DefaultValueExplanation("Do not add a Data Source")]
    public IStep<StringStream>? DataSourcePath { get; set; } = null!;

    /// <summary>
    /// The Path to the Ingest Profile Name.
    /// If this is not given, the data source will be added but not ingested.
    /// If this is an empty string then the default ingest profile will be used.
    /// </summary>
    [StepProperty(5)]
    [DefaultValueExplanation("The data source will not be ingested")]
    public IStep<StringStream>? IngestProfileName { get; set; }

    /// <inheritdoc />
    public override async ValueTask<Result<IReadOnlyList<string>, IError>> GetArguments(
        IStateMonad stateMonad,
        CancellationToken cancellationToken)
    {
        var argResult = await stateMonad.RunStepsAsync(
            CaseName.WrapStringStream(),
            CaseBaseDirectory.WrapStringStream(),
            CaseType.WrapNullable(),
            DataSourcePath.WrapNullable(StepMaps.String()),
            IngestProfileName.WrapNullable(StepMaps.String()),
            cancellationToken
        );

        if (argResult.IsFailure)
            return argResult.ConvertFailure<IReadOnlyList<string>>();

        var (caseName, caseBaseDir, caseType, dataSourcePath, ingestProfileName)
            = argResult.Value;

        var arguments = new List<string>
        {
            "--nosplash",
            "--createCase",
            "--caseName",
            caseName,
            "--caseBaseDir",
            caseBaseDir
        };

        if (caseType.HasValue)
            arguments.AddRange(new[] { $"--caseType={caseType.Value.Value.GetDisplayName()}", });

        if (dataSourcePath.HasValue)
        {
            arguments.AddRange(
                new[] { "--addDataSource", $"--dataSourcePath", dataSourcePath.Value }
            );

            if (ingestProfileName.HasValue)
            {
                arguments.Add("--runIngest");

                if (!string.IsNullOrWhiteSpace(ingestProfileName.Value))
                    arguments.Add(ingestProfileName.Value);
            }
        }

        return arguments;
    }
}
