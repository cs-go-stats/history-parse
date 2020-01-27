# v0.1.8
# v0.1.7
# v0.1.6
## Changed
* Fixed a bug with wrong already parsed bound find condition that interrupted parse in case of new historical entry.

# v0.1.5
## Added
* Database migrations.
* Registration of already parsed history.
* Job for weekly forced history parse.
## Changed
* Refactored history parse settings.
## Removed
* `HistoryParse` aggregate.

# v0.1.4
## Changed
* Fixed startup issues related to Core functionality.

# v0.1.3
## Changed
* Added reference to Core package that allowed to simplify startup.

# v0.1.2
## Added
* Scheduling for history parse.

# v0.1.1
## Added
* Scheduling for history parse.

# v0.1.0
## Added
* Basic implementation of history parse and message publish for each match; configurable conditions for early process terminating.