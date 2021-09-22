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
    public IStep<AutopsyCaseType>? CaseType { get; set; } = null;

    /// <summary>
    /// The Path to the Data Source to add
    /// </summary>
    [StepProperty(4)]
    [DefaultValueExplanation("Do not add a Data Source")]
    public IStep<StringStream>? DataSourcePath { get; set; } = null!;

    /// <summary>
    /// The Path to the Data Source.
    /// If the DataSourcePath is not given then this will be ignored.
    /// </summary>
    [StepProperty(5)]
    [DefaultValueExplanation("The name of the ingest profile to use")]
    public IStep<StringStream>? IngestProfileName { get; set; }

    /// <inheritdoc />
    public override async Task<Result<IReadOnlyList<string>, IError>> GetArguments(
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
            arguments.AddRange(new[] { $"--caseType={caseType.Value.GetDisplayName()}", });

        if (dataSourcePath.HasValue)
        {
            arguments.AddRange(
                new[] { "--addDataSource", $"--dataSourcePath", dataSourcePath.Value }
            );

            arguments.Add("--runIngest");

            if (!string.IsNullOrWhiteSpace(ingestProfileName.Value))
                arguments.Add(ingestProfileName.Value);
        }

        return arguments;
    }
}

}
