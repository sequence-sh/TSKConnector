# Sequence Connector for The Sleuth Kit®

[Sequence®](https://sequence.sh) is a collection of libraries for
automation of cross-application e-discovery and forensic workflows.

This connector contains Steps that interact with the
[Autopsy Console application](http://sleuthkit.org/autopsy/docs/user-docs/4.19.0/command_line_ingest_page.html).

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

## Settings

The TSK Connector requires additional configuration which can be
provided using the `settings` key in `connectors.json`.

### Supported Settings

| Name        | Required |   Type   | Description                            |
| :---------- | :------: | :------: | :------------------------------------- |
| AutopsyPath |    ✔     | `string` | The Path the to the Autopsy Executable |

### Example Settings

```json
"Sequence.Connectors.TSK": {
  "id": "Sequence.Connectors.TSK",
  "enable": true,
  "version": "0.16.0",
  "settings": {
    "AutopsyPath": "C:\\Program Files\\Autopsy-4.19.1\\bin\\autopsy64.exe"
  }
}
```

# Documentation

https://sequence.sh

# Download

https://sequence.sh/download

# Try SCL and Core

https://sequence.sh/playground

# Package Releases

Can be downloaded from the [Releases page](https://gitlab.com/sequence/connectors/tsk/-/releases).

# NuGet Packages

Release nuget packages are available from [nuget.org](https://www.nuget.org/profiles/Sequence).
