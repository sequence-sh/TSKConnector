using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Reductech.EDR.Core;
using Reductech.EDR.Core.Internal;
using Reductech.EDR.Core.Internal.Errors;
using Reductech.EDR.Core.Util;

namespace Reductech.EDR.Connectors.TSK.Steps
{

/// <summary>
/// List all Data Sources in a TSK Case.
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

}
