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

namespace Reductech.EDR.Connectors.TSK.Steps
{

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
                            $"{caseDirectory}",
                            "--addDataSource",
                            $"--dataSourcePath",
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

}
