using Reductech.Sequence.ConnectorManagement.Base;
using Reductech.Sequence.Core.Internal.Errors;
using Entity = Reductech.Sequence.Core.Entity;

namespace Reductech.Sequence.Connectors.TSK;

/// <summary>
/// Contains helper methods for TSK settings
/// </summary>
public static class SettingsHelpers
{
    private static readonly string RelativityConnectorKey =
        typeof(TSKSettings).Assembly.GetName().Name!;

    /// <summary>
    /// Try to get a TesseractSettings from a list of Connector Informations
    /// </summary>
    public static Result<TSKSettings, IErrorBuilder> TryGetTSKSettings(this Entity settings)
    {
        var connectorEntityValue = settings.TryGetValue(
            new EntityNestedKey(
                StateMonad.ConnectorsKey,
                RelativityConnectorKey,
                nameof(ConnectorSettings.Settings)
            )
        );

        if (connectorEntityValue.HasNoValue ||
            connectorEntityValue.Value is not Entity nestedEntity)
            return ErrorCode.MissingStepSettings.ToErrorBuilder(RelativityConnectorKey);

        var connectorSettings =
            EntityConversionHelpers.TryCreateFromEntity<TSKSettings>(nestedEntity);

        return connectorSettings;
    }
}
