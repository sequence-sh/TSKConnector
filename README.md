# Sequence® Connector for The Sleuth Kit

[Reductech Sequence®](https://gitlab.com/reductech/sequence) is a collection of
libraries that automates cross-application e-discovery and forensic workflows.

This connector contains Steps that interact with the [Autopsy Console application](http://sleuthkit.org/autopsy/docs/user-docs/4.19.0/command_line_ingest_page.html).

## Steps

|           Step           | Description                               | Result Type |
| :----------------------: | :---------------------------------------- | :---------: |
|  `AutopsyCreateNewCase`  | Creates a new Autopsy Case.               |   `Unit`    |
|  `AutopsyAddDataSource`  | Add a Data Source to an Autopsy Case.     |   `Unit`    |
| `AutopsyGenerateReports` | Generate Reports for an Autopsy Case.     |   `Unit`    |
| `AutopsyListDataSources` | List all Data Sources in an Autopsy Case. |   `Unit`    |

## Examples

Create a new Case and add data to it

```scala
- AutopsyCreateNewCase
    CaseName: "TestCase"
    CaseBaseDirectory: "C:\\Cases"
    CaseType: AutopsyCaseType.single
    DataSourcePath: "C:\\Data\\loadfile_0001-10001.dat"
    IngestProfileName: ""
```

This will create a new case in `c:\Cases`.
The Case name will be 'TestCase' with the current date and time appended to it.

# Releases

Can be downloaded from the [Releases page](https://gitlab.com/reductech/sequence/connectors/tsk/-/releases).

# NuGet Packages

Are available in the [Reductech Nuget feed](https://gitlab.com/reductech/nuget/-/packages).
