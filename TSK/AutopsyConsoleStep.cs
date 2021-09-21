using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Reductech.EDR.Core;
using Reductech.EDR.Core.ExternalProcesses;
using Reductech.EDR.Core.Internal;
using Reductech.EDR.Core.Internal.Errors;
using Reductech.EDR.Core.Util;

namespace Reductech.EDR.Connectors.TSK
{

/// <summary>
/// A step that runs an Autopsy command
/// </summary>
public abstract class AutopsyConsoleStep : CompoundStep<Unit>
{
    /// <summary>
    /// The Command arguments
    /// </summary>
    public abstract Task<Result<IReadOnlyList<string>, IError>> GetArguments(
        IStateMonad stateMonad,
        CancellationToken cancellationToken);

    /// <inheritdoc />
    protected override async Task<Result<Unit, IError>> Run(
        IStateMonad stateMonad,
        CancellationToken cancellationToken)
    {
        var settingsResult = stateMonad.Settings.TryGetTSKSettings();

        if (settingsResult.IsFailure)
            return settingsResult.ConvertFailure<Unit>().MapError(x => x.WithLocation(this));

        var arguments = await GetArguments(stateMonad, cancellationToken);

        if (arguments.IsFailure)
            return arguments.ConvertFailure<Unit>();

        var result = await
            stateMonad.ExternalContext.ExternalProcessRunner.RunExternalProcess(
                    settingsResult.Value.AutopsyPath,
                    IgnoreNoneErrorHandler.Instance,
                    arguments.Value,
                    new Dictionary<string, string>(), //Do we need to add java path
                    Encoding.UTF8,
                    stateMonad,
                    this,
                    cancellationToken
                )
                .MapError(x => x.WithLocation(this));

        return result;
    }
}

}
