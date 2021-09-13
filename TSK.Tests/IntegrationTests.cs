using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Reductech.EDR.ConnectorManagement.Base;
using Reductech.EDR.Core.Abstractions;
using Reductech.EDR.Core.Internal;
using Reductech.EDR.Core.Internal.Serialization;
using Reductech.EDR.Core.TestHarness;
using Xunit;
using Xunit.Abstractions;
using static Reductech.EDR.Core.TestHarness.StaticHelpers;

namespace Reductech.EDR.Connectors.TSK.Tests
{

[AutoTheory.UseTestOutputHelper]
public partial class IntegrationTests
{
    public const string SkipAll = "";

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void TestCreateNewCase()
    {
        var sequence = new TSKCreateNewCase()
        {
            CaseName          = Constant("IntegrationTestCase"),
            CaseBaseDirectory = Constant(@"C:\Users\wainw\source\stuff"),
            CaseType          = Constant(TSKCaseType.single)
        };

        await TestSCLSequence(sequence);
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void TestOpenCase()
    {
        var sequence = new TSKOpenCase()
        {
            CaseDirectory = Constant(
                @"C:\Users\wainw\source\stuff\IntegrationTestCase_2021_09_13_15_43_05\IntegrationTestCase.aut"
            )
        };

        await TestSCLSequence(sequence);
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void TestAddDataSource()
    {
        var sequence = new TSKAddDataSource()
        {
            CaseDirectory =
                Constant(@"C:\Users\wainw\source\stuff\IntegrationTestCase_2021_09_13_15_43_05"),
            DataSourcePath =
                Constant("C:\\Users\\wainw\\source\\dataExamples\\Pictures\\Mark in the Park.jpg"),
            IngestProfileName = null
        };

        await TestSCLSequence(sequence);
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void TestGenerateReports()
    {
        var sequence = new TSKGenerateReports()
        {
            CaseDirectory =
                Constant(@"C:\Users\wainw\source\stuff\IntegrationTestCase_2021_09_13_15_43_05"),
            ProfileName = null
        };

        await TestSCLSequence(sequence);
    }

    private async Task TestSCLSequence(IStep step)
    {
        var logger =
            TestOutputHelper.BuildLogger(new LoggingConfig() { LogLevel = LogLevel.Trace });

        var scl = step.Serialize();
        logger.LogInformation(scl);

        var sfs = StepFactoryStore.Create(
            new ConnectorData(
                new ConnectorSettings()
                {
                    Id     = "Reductech.EDR.Connectors.TSK",
                    Enable = true,
                    Settings = new TSKSettings()
                    {
                        AutopsyPath = @"C:\Program Files\Autopsy-4.19.1\bin\autopsy64.exe"
                    }.ToDictionary()
                },
                typeof(TSKSettings).Assembly
            )
        );

        var runner = new SCLRunner(
            logger,
            sfs,
            ExternalContext.Default
        );

        var r = await runner.RunSequenceFromTextAsync(
            scl,
            new Dictionary<string, object>(),
            CancellationToken.None
        );

        r.ShouldBeSuccessful();
    }
}

}
