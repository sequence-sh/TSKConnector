namespace Reductech.EDR.Connectors.TSK;

/// <summary>
/// Base step for steps which open an existing case
/// </summary>
public abstract class ExistingCaseStep : AutopsyConsoleStep
{
    /// <summary>
    /// The Directory containing the case
    /// </summary>
    [StepProperty(1)]
    [Required]
    public IStep<StringStream> CaseDirectory { get; set; } = null!;
}
