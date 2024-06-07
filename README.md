# PowerShell Command History

Console application allow copy to clipboard selected item from list all of the commands that have been run from PowerShell

![PowerShell-Command-History](https://user-images.githubusercontent.com/35919087/163270057-0306d46f-588a-47ea-95ae-6fadefb3a424.gif)

## Features
- Returns list of all the commands that have been run from PowerShell terminal
- Filter the commands
- Set page size list
- Copy selected command to the clipboard
- Save favourites commands
- Integration with ChatGPT 

## Installation
Put the published version of the application into the program files directory e.g.
```sh
C:\Program Files\PSCH
```
Add an environment variable by command in PowerShell as Administrator
```sh
[Environment]::SetEnvironmentVariable("PATH", $Env:PATH + ";C:\Program Files\PSCH", [EnvironmentVariableTarget]::Machine)
```

To use ChatGPT, you need to add an OPEN_AI_API_KEY key to the environment variable. Your API key can be obtained from [here](https://platform.openai.com/account/api-keys)
```sh
[System.Environment]::SetEnvironmentVariable("OPEN_AI_API_KEY", "<Your-API-key>", "Machine")
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
Start conversation with ChatGPT. Default model is GPT-4o.
```sh
psch -gpt
```

Set gpt-3.5-turbo model and start conversation with ChatGPT.
List of models can be obtained from [here](https://platform.openai.com/docs/models)
```sh
psch -gpt gpt-3.5-turbo
```

### Simple usage `psch -gpt`
![psch-gpt](https://github.com/MaciejTrudnos/PowerShell-Command-History/assets/35919087/20f4c112-bc14-427a-bed9-6ed371fc9f51)

## Tech
- [Sharprompt](https://github.com/shibayan/Sharprompt) - Interactive command-line based application framework for C#
- [.NET SDK for OpenAI](https://github.com/betalgo/openai) - A .NET SDK for accessing OpenAI's API, provided as a community library.

## License
[Released under the MIT license.](https://github.com/MaciejTrudnos/PowerShell-Command-History/blob/master/LICENSE)