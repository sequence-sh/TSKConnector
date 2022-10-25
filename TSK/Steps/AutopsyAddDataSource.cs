using Reductech.Sequence.Core.Internal.Errors;

namespace Reductech.Sequence.Connectors.TSK.Steps;

/// <summary>
/// Add a Data Source to an Autopsy Case
/// </summary>
public sealed class AutopsyAddDataSource : ExistingCaseStep
{
    /// <summary>
    /// The Path to the Data Source
    /// </summary>
    [StepProperty(2)]
    [Required]
    public IStep<StringStream> DataSourcePath { get; set; } = null!;

    /// <summary>
    /// The Path to the Ingest Profile Name.
    /// If this is not given, the data source will be added but not ingested.
    /// If this is an empty string then the default ingest profile will be used.
    /// </summary>
    [StepProperty(3)]
    [DefaultValueExplanation("The data source will not be ingested")]
    public IStep<StringStream>? IngestProfileName { get; set; }

    /// <inheritdoc />
    public override IStepFactory StepFactory { get; } =
        new SimpleStepFactory<AutopsyAddDataSource, Unit>();

    /// <inheritdoc />
    public override async ValueTask<Result<IReadOnlyList<string>, IError>> GetArguments(
        IStateMonad stateMonad,
        CancellationToken cancellationToken)
    {
        return await stateMonad.RunStepsAsync(
                    CaseDirectory.WrapStringStream(),
                    DataSourcePath.WrapStringStream(),
                    IngestProfileName.WrapNullable(StepMaps.String()),
                    cancellationToken
                )
                .Map(
                    r =>
                    {
                        var (caseDirectory, dataSourcePath, ingestProfileName) = r;

                        var list = new List<string>()
                        {
                            "--nosplash",
                            "--caseDir",
                            $"{caseDirectory}",
                            "--addDataSource",
                            "--dataSourcePath",
                            dataSourcePath
                        };

                        if (ingestProfileName.HasValue)
                        {
                            list.Add("--runIngest");

                            if (!string.IsNullOrWhiteSpace(ingestProfileName.Value))
                                list.Add(ingestProfileName.Value);
                        }

                        return list as IReadOnlyList<string>;
                    }
                )
            ;
    }
}
