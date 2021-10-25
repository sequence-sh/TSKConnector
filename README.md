# EDR Connector for TSK

[Reductech EDR](https://gitlab.com/reductech/edr) is a collection of
libraries that automates cross-application e-discovery and forensic workflows.

This connector contains Steps that interact with the [Autopsy Console application](http://sleuthkit.org/autopsy/docs/user-docs/4.19.0/command_line_ingest_page.html). 

## Steps

|         Step          | Description                                    | Result Type |
| :-------------------: | :--------------------------------------------- | :---------: |
| `AutopsyCreateNewCase` | Creates a new Autopsy Case. |  `Unit`   |
| `AutopsyAddDataSource` | Add a Data Source to an Autopsy Case. |  `Unit`   |
| `AutopsyGenerateReports` | Generate Reports for an Autopsy Case. |  `Unit`   |
| `AutopsyListDataSources` | List all Data Sources in an Autopsy Case. |  `Unit`   |

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


### [Try TSK Connector](https://gitlab.com/reductech/edr/edr/-/releases)

Using [EDR](https://gitlab.com/reductech/edr/edr),
the command line tool for running Sequences.

## Documentation

Documentation is available here: https://docs.reductech.io

## E-discovery Reduct

The TSK Connector is part of a group of projects called
[E-discovery Reduct](https://gitlab.com/reductech/edr)
which consists of a collection of [Connectors](https://gitlab.com/reductech/edr/connectors)
and a command-line application for running Sequences, called
[EDR](https://gitlab.com/reductech/edr/edr/-/releases).

# Releases

Can be downloaded from the [Releases page](https://gitlab.com/reductech/edr/connectors/tsk/-/releases).

# NuGet Packages

Are available in the [Reductech Nuget feed](https://gitlab.com/reductech/nuget/-/packages).
