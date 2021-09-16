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
/// Open a Case in Autopsy
/// </summary>
public sealed class TSKOpenCase : ExistingCaseStep
{
    /// <inheritdoc />
    public override IStepFactory StepFactory { get; } = new SimpleStepFactory<TSKOpenCase, Unit>();

    /// <inheritdoc />
    public override Task<Result<IReadOnlyList<string>, IError>> GetArguments(
        IStateMonad stateMonad,
        CancellationToken cancellationToken)
    {
        return CaseDirectory
            .WrapStringStream()
            .Run(stateMonad, cancellationToken)
            .Map(x => new List<string>() { $"{x}" } as IReadOnlyList<string>);
    }
}

}
