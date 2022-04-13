# PowerShell Command Hisory
Console application to allow user look list all of the command that have been run from PowerShell terminal.

## Features
- Returns list of all the command that have been run from PowerShell terminal
- Allows filter the commands in the list
- Allows set page size list
- Copies the selected command to the clipboard

## Installation
Put the published version of the application into the program files directory e.g.
```sh
C:\Program Files\PSCH
```
Add an environment variable by command in PowerShell as Administrator
```sh
[Environment]::SetEnvironmentVariable("PATH", $Env:PATH + ";C:\Program Files\PSCH", [EnvironmentVariableTarget]::Machine)
```

## Command usage
Print help
```sh
psch -h
```
Print version
```sh
psch -v
```
Set page size as 15
```sh
psch -s 15
```

## Tech
- [Sharprompt](https://github.com/shibayan/Sharprompt) - Interactive command-line based application framework for C#

## License
Released under the MIT license.