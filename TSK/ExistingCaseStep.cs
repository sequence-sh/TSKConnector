using System.ComponentModel.DataAnnotations;
using Reductech.EDR.Core;
using Reductech.EDR.Core.Attributes;
using Reductech.EDR.Core.Internal;

namespace Reductech.EDR.Connectors.TSK
{

/// <summary>
/// Base step for steps which open an existing case
/// </summary>
public abstract class ExistingCaseStep : TSKConsoleStep
{
    /// <summary>
    /// The Directory containing the case
    /// </summary>
    [StepProperty(1)]
    [Required]
    public IStep<StringStream> CaseDirectory { get; set; } = null!;
}

}
