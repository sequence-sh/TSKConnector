using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Reductech.EDR.Core;
using Reductech.EDR.Core.Attributes;
using Reductech.EDR.Core.ExternalProcesses;
using Reductech.EDR.Core.Internal;
using Reductech.EDR.Core.Internal.Errors;
using Reductech.EDR.Core.Util;
using Entity = Reductech.EDR.Core.Entity;

namespace Reductech.EDR.Connectors.TSK.Steps
{

/// <summary>
/// List all Data Sources in a TSK Case
/// </summary>
public sealed class TSKListDataSources : CompoundStep<Array<Entity>>
{
    /// <summary>
    /// The Directory containing the case
    /// </summary>
    [StepProperty(1)]
    [Required]
    public IStep<StringStream> CaseDirectory { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task<Result<Array<Entity>, IError>> Run(
        IStateMonad stateMonad,
        CancellationToken cancellationToken)
    {
        var settingsResult = stateMonad.Settings.TryGetTSKSettings();

        if (settingsResult.IsFailure)
            return settingsResult.ConvertFailure<Array<Entity>>()
                .MapError(x => x.WithLocation(this));

        var arguments = await GetArguments(stateMonad, cancellationToken);

        if (arguments.IsFailure)
            return arguments.ConvertFailure<Array<Entity>>();

        var startResult =
            stateMonad.ExternalContext.ExternalProcessRunner
                .StartExternalProcess(
                    settingsResult.Value.AutopsyPath,
                    arguments.Value,
                    new Dictionary<string, string>(),
                    Encoding.UTF8,
                    stateMonad,
                    this
                )
                .MapError(x => x.WithLocation(this));

        if (startResult.IsFailure)
            return startResult.ConvertFailure<Array<Entity>>();

        var data =
            await
                startResult.Value.OutputChannel.ReadAllAsync(cancellationToken)
                    .Select(x => TryConvertToEntity(x.line, x.source))
                    .ToListAsync(cancellationToken);

        startResult.Value.Dispose();

        var result = data.Where(x => x.HasValue)
            .Select(x => x.Value)
            .Combine(ErrorBuilderList.Combine)
            .MapError(x => x.WithLocation(this))
            .Map(x => x.ToSCLArray());

        return result;
    }

    private static Maybe<Result<Entity, IErrorBuilder>> TryConvertToEntity(
        string line,
        StreamSource source)
    {
        if (source == StreamSource.Error)
            return Maybe<Result<Entity, IErrorBuilder>>.From(
                ErrorCode.ExternalProcessError.ToErrorBuilder(line)
            );

        return Maybe<Result<Entity, IErrorBuilder>>.From(Entity.Create(("Data", line)));
    }

    /// <summary>
    /// The Command arguments
    /// </summary>
    public Task<Result<IReadOnlyList<string>, IError>> GetArguments(
        IStateMonad stateMonad,
        CancellationToken cancellationToken)
    {
        return CaseDirectory
            .WrapStringStream()
            .Run(stateMonad, cancellationToken)
            .Map(
                x => new List<string>() { "--nosplash", $"{x}", "--listAllDataSources" } as
                    IReadOnlyList<string>
            );
    }

    /// <inheritdoc />
    public override IStepFactory StepFactory =>
        new SimpleStepFactory<TSKListDataSources, Array<Entity>>();
}

}
