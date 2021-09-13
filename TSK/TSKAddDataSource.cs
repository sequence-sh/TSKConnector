using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Reductech.EDR.Core;
using Reductech.EDR.Core.Attributes;
using Reductech.EDR.Core.Internal;
using Reductech.EDR.Core.Internal.Errors;
using Reductech.EDR.Core.Util;

namespace Reductech.EDR.Connectors.TSK
{

public sealed class TSKGenerateReports : ExistingCaseStep
{
    /// <inheritdoc />
    public override IStepFactory StepFactory { get; } =
        new SimpleStepFactory<TSKGenerateReports, Unit>();

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

                        var list = new List<string>() { $"--caseDir=\"{caseDirectory}\"", };

                        if (profileName.HasValue)
                            list.Add($"--runIngest=\"{profileName.Value}\"");
                        else
                            list.Add("--runIngest");

                        return list as IReadOnlyList<string>;
                    }
                )
            ;
    }
}

/// <summary>
/// Add a Data Source
/// </summary>
public sealed class TSKAddDataSource : ExistingCaseStep
{
    /// <summary>
    /// The Path to the Data Source
    /// </summary>
    [StepProperty(2)]
    [Required]
    public IStep<StringStream> DataSourcePath { get; set; } = null!;

    /// <summary>
    /// The Path to the Data Source.
    /// If this is empty the default ingest profile will be used instead
    /// </summary>
    [StepProperty(3)]
    [DefaultValueExplanation("The name of the ingest profile to use")]
    public IStep<StringStream>? IngestProfileName { get; set; }

    /// <inheritdoc />
    public override IStepFactory StepFactory { get; } =
        new SimpleStepFactory<TSKAddDataSource, Unit>();

    /// <inheritdoc />
    public override async Task<Result<IReadOnlyList<string>, IError>> GetArguments(
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
                            $"--caseDir=\"{caseDirectory}\"",
                            "--addDataSource",
                            $"--dataSourcePath=\"{dataSourcePath}\""
                        };

                        if (ingestProfileName.HasValue)
                        {
                            if (string.IsNullOrWhiteSpace(ingestProfileName.Value))
                                list.Add("--runIngest");
                            else
                                list.Add($"--runIngest=\"{ingestProfileName.Value}\"");
                        }

                        return list as IReadOnlyList<string>;
                    }
                )
            ;
    }
}

}
