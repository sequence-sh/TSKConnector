using System.Runtime.Serialization;

namespace Reductech.Sequence.Connectors.TSK;

/// <summary>
/// Settings for the TSK Connector
/// </summary>
[DataContract]
public sealed class TSKSettings : IEntityConvertible
{
    /// <summary>
    /// The Path the to the Autopsy Executable
    /// </summary>
    [DataMember]
    public string AutopsyPath { get; set; }

    /// <summary>
    /// Converts this TSK settings object to a dictionary
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object> { { nameof(AutopsyPath), AutopsyPath }, };
    }
}
