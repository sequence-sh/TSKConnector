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
public sealed class TSKCreateNewCase : TSKConsoleStep
{
    /// <inheritdoc />
    public override IStepFactory StepFactory { get; } =
        new SimpleStepFactory<TSKCreateNewCase, Unit>();

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
    public IStep<TSKCaseType>? CaseType { get; set; } = null;

    /// <inheritdoc />
    public override async Task<Result<IReadOnlyList<string>, IError>> GetArguments(
        IStateMonad stateMonad,
        CancellationToken cancellationToken)
    {
        var argResult = await stateMonad.RunStepsAsync(
            CaseName.WrapStringStream(),
            CaseBaseDirectory.WrapStringStream(),
            CaseType.WrapNullable(),
            cancellationToken
        );

        if (argResult.IsFailure)
            return argResult.ConvertFailure<IReadOnlyList<string>>();

        var (caseName, caseBaseDir, caseType) = argResult.Value;

        var arguments = new List<string>
        {
            "--nosplash", "--createCase", $"--caseName={caseName}", $"--caseBaseDir={caseBaseDir}",
        };

        if (caseType.HasValue)
            arguments.AddRange(new[] { $"--caseType={caseType.Value.GetDisplayName()}", });

        return arguments;
    }
}

}
