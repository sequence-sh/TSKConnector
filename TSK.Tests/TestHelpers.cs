using System.Reflection;
using Reductech.EDR.ConnectorManagement.Base;
using Reductech.EDR.Connectors.TSK.Steps;
using Reductech.EDR.Core.Abstractions;
using Reductech.EDR.Core.Internal;
using Reductech.EDR.Core.TestHarness;

namespace Reductech.EDR.Connectors.TSK.Tests
{

public static class TestHelpers
{
    public static readonly TSKSettings TestTSKSettings = new() { AutopsyPath = "C:/AutopsyTest" };

    public static T WithTestTSKSettings<T>(this T stepCase)
        where T : ICaseThatExecutes
    {
        var r = stepCase.WithStepFactoryStore(
            StepFactoryStore.TryCreate(
                    ExternalContext.Default,
                    new ConnectorData(
                        new ConnectorSettings()
                        {
                            Enable   = true,
                            Id       = TSKAssembly.GetName().Name!,
                            Settings = TestTSKSettings.ToDictionary()
                        },
                        TSKAssembly
                    )
                )
                .Value
        );

        return r;
    }

    public static Assembly TSKAssembly { get; } = typeof(AutopsyCreateNewCase).Assembly;
}

}
