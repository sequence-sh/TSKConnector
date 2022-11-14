# v0.18.0 (2022-11-14)

## Issues Closed in this Release

### Other

- Update namespace and paths after move to Sequence group #123

# v0.17.0 (2022-08-29)

Maintenance release - dependency updates only.

# v0.16.0 (2022-07-13)

- Enabled [Source Link](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink)
- Enabled publish to [Nuget.org](https://www.nuget.org) including symbols
- Update Core to v0.16.0

# v0.15.0 (2022-05-27)

Maintenance release - dependency updates only.

# v0.14.0 (2022-03-25)

Maintenance release - dependency updates only.

# v0.13.0 (2022-01-16)

EDR is now Sequence. The following has changed:

- The GitLab group has moved to https://gitlab.com/sequence
- The root namespace is now `Sequence`
- Our documentation site has moved to https://sequence.sh

Everything else is still the same - automation, simplified.

The project has now been updated to use .NET 6.

## Issues Closed in this Release

### Maintenance

- Rename EDR to Sequence #19
- Update Core to support SCLObject Types #16
- Upgrade to use .net 6 #15

# v0.12.0 (2021-11-26)

Initial release. Version numbers are aligned with [Core](https://gitlab.com/reductech/edr/core/-/releases).

## Summary of Changes

### Connector Updates

- Added `AutopsyAddDataSource`
- Added `AutopsyCreateNewCase`
- Added `AutopsyGenerateReports`
- Added `AutopsyListDataSources`
- Added `AutopsyOpenCase`

## Issues Closed in this Release

### New Features

- AutopsyCreateNewCase should be able to add a data source at the same time #3
- Create Steps which use the Autopsy Command line #2

### Maintenance

- Set Up Connector #1

### Documentation

- Update README with Steps and any required settings #5

