using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Reductech.EDR.Core.Connectors;
using Reductech.EDR.Core.Internal.Errors;

namespace Reductech.EDR.Connectors.TSK
{

/// <summary>
/// For injecting the connector context
/// </summary>
public sealed class ConnectorInjection : IConnectorInjection
{
    /// <inheritdoc />
    public Result<IReadOnlyCollection<(string Name, object Context)>, IErrorBuilder>
        TryGetInjectedContexts()
    {
        return Result.Success<IReadOnlyCollection<(string Name, object Context)>, IErrorBuilder>(
            new List<(string Name, object Context)>()
        );
    }
}

}
