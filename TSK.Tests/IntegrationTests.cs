using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Reductech.EDR.ConnectorManagement.Base;
using Reductech.EDR.Connectors.TSK.Steps;
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
    public const string SkipAll = "manual";

    public const string TestCaseName = "IntegrationTestCase";

    public const string TestCaseBaseDirectory = @"C:\Users\wainw\source\stuff";

    public const string TestDataSourcePath =
        @"C:\Users\wainw\source\dataExamples\loadfile_0001-10001.dat";

    public static readonly string TestCase =
        @$"{TestCaseBaseDirectory}\{TestCaseName}_2021_09_16_16_56_47\{TestCaseName}.aut";

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void CreateNewCase()
    {
        var sequence = new AutopsyCreateNewCase()
        {
            CaseName          = Constant(TestCaseName),
            CaseBaseDirectory = Constant(TestCaseBaseDirectory),
            CaseType          = Constant(TSKCaseType.single)
        };

        await TestSCLSequence(sequence);
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void OpenCase()
    {
        var sequence = new AutopsyOpenCase() { CaseDirectory = Constant(TestCase) };

        await TestSCLSequence(sequence);
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void AddDataSource()
    {
        var sequence = new AutopsyAddDataSource()
        {
            CaseDirectory =
                Constant(TestCase),
            DataSourcePath =
                Constant(TestDataSourcePath),
            IngestProfileName = Constant("")
        };

        await TestSCLSequence(sequence);
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void GenerateReports()
    {
        var sequence = new AutopsyGenerateReports()
        {
            CaseDirectory =
                Constant(TestCase),
            ProfileName = null
        };

        await TestSCLSequence(sequence);
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void TestListDataSources()
    {
        var sequence = new AutopsyListDataSources()
        {
            CaseDirectory =
                Constant(TestCase),
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
