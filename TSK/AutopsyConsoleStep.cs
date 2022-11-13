using System.Text;
using Sequence.Core.ExternalProcesses;
using Sequence.Core.Internal.Errors;

namespace Sequence.Connectors.TSK;

/// <summary>
/// A step that runs an Autopsy command
/// </summary>
public abstract class AutopsyConsoleStep : CompoundStep<Unit>
{
    /// <summary>
    /// The Command arguments
    /// </summary>
    public abstract ValueTask<Result<IReadOnlyList<string>, IError>> GetArguments(
        IStateMonad stateMonad,
        CancellationToken cancellationToken);

    /// <inheritdoc />
    protected override async ValueTask<Result<Unit, IError>> Run(
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

        return result; //TODO check for message "Job processing task finished"
    }
}
