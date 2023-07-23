# PowerShell Command History
[![Build](https://github.com/MaciejTrudnos/PowerShell-Command-History/workflows/Build/badge.svg)](https://github.com/MaciejTrudnos/PowerShell-Command-History/actions/workflows/dotnet.yml)

Console application allow copy to clipboard selected item from list all of the command that have been run from PowerShell terminal.

![PowerShell-Command-History](https://user-images.githubusercontent.com/35919087/163270057-0306d46f-588a-47ea-95ae-6fadefb3a424.gif)

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
Save command
```sh
psch -s
```
Remove command
```sh
psch -r
```
Saved commands list
```sh
psch -f
```

## Tech
- [Sharprompt](https://github.com/shibayan/Sharprompt) - Interactive command-line based application framework for C#

## License
[Released under the MIT license.](https://github.com/MaciejTrudnos/PowerShell-Command-History/blob/master/LICENSE)
