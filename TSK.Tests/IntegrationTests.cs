using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Divergic.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Sequence.ConnectorManagement.Base;
using Sequence.Connectors.TSK.Steps;
using Sequence.Core.Abstractions;
using Sequence.Core.Internal.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Sequence.Connectors.TSK.Tests;

[AutoTheory.UseTestOutputHelper]
public partial class IntegrationTests
{
    public const string SkipAll = "manual";

    public const string TestCaseName = "IntegrationTestCase";

    public const string TestCaseBaseDirectory = @"C:\Users\wainw\source\stuff";

    public const string TestDataSourcePath =
        @"C:\Users\wainw\source\dataExamples\loadfile_0001-10001.dat";

    public static string GetTestCasePath()
    {
        var dir = Directory.EnumerateDirectories(
                TestCaseBaseDirectory,
                TestCaseName + "*"
            )
            .FirstOrDefault();

        if (dir is null)
            throw new Exception($"Cannot find case {TestCaseName}");

        return dir;
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void CreateNewCase()
    {
        var sequence = new AutopsyCreateNewCase()
        {
            CaseName          = Constant(TestCaseName),
            CaseBaseDirectory = Constant(TestCaseBaseDirectory),
            CaseType          = Constant(AutopsyCaseType.single)
        };

        await TestSCLSequence(sequence);
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void CreateNewCaseAndAddData()
    {
        var sequence = new AutopsyCreateNewCase()
        {
            CaseName          = Constant(TestCaseName),
            CaseBaseDirectory = Constant(TestCaseBaseDirectory),
            CaseType          = Constant(AutopsyCaseType.single),
            DataSourcePath =
                Constant(TestDataSourcePath),
            IngestProfileName = Constant("")
        };

        await TestSCLSequence(sequence);
    }

    [Fact(Skip = SkipAll)]
    [Trait("Category", "Integration")]
    public async void AddDataSource()
    {
        var sequence = new AutopsyAddDataSource()
        {
            CaseDirectory =
                Constant(GetTestCasePath()),
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
                Constant(GetTestCasePath()),
            ProfileName = Constant("html")
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
                Constant(GetTestCasePath()),
        };

        await TestSCLSequence(sequence);
    }

    private async Task TestSCLSequence(IStep step)
    {
        var logger =
            TestOutputHelper.BuildLogger(new LoggingConfig() { LogLevel = LogLevel.Trace });

        var scl = step.Serialize(SerializeOptions.Serialize);
        logger.LogInformation(scl);

        var sfs = StepFactoryStore.TryCreate(
                ExternalContext.Default,
                new ConnectorData(
                    new ConnectorSettings()
                    {
                        Id     = "Sequence.Connectors.TSK",
                        Enable = true,
                        Settings = new TSKSettings()
                        {
                            AutopsyPath =
                                @"C:\Program Files\Autopsy-4.19.1\bin\autopsy64.exe"
                        }.ToDictionary()
                    },
                    typeof(TSKSettings).Assembly
                )
            )
            .Value;

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
