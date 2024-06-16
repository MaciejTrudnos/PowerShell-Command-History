using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using PSCH.Data;
using PSCH.Model;
using PSCH.Services;
using Sharprompt;
using System.Reflection;
using System.Windows.Forms;

class Program
{
    private readonly static string _filePath = Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Roaming\Microsoft\Windows\PowerShell\PSReadline\ConsoleHost_history.txt");
    private readonly static DataContext _dbContext = new();
    private readonly static List<ChatMessage> _messages = 
    [
        ChatMessage.FromSystem("You are a helpful assistant."),
        ChatMessage.FromSystem("Return only powershell command in one line.")
    ];

[STAThread]
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException($"{_filePath}");

            var psch = GetPowerShellCommandHistory();

            var selectedCommand = SelectCommand("Filter:", psch);

            Clipboard.SetText(selectedCommand);

            ClearPSCH(psch);

            return;
        }

        if (args[0] == "-gpt")
        {
            var apiKey = Environment
                .GetEnvironmentVariable("OPEN_AI_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("OPEN_AI_API_KEY Environment variable value not found");

            var model = Models.Gpt_4o;
            
            if (args.Length == 2)
                model = args[1];

            var gpt = new ChatGptService(apiKey, model);

            var stopChat = false;
            int chatCounter = 0;

            do
            {
                if (chatCounter > 100)
                    throw new IndexOutOfRangeException("Too Many Requests");

                var message = Prompt.Input<string>("", placeholder: "Send a message...");

                _messages.Add(ChatMessage.FromUser(message));

                var request = gpt.SendMessage(_messages);

                request.Wait();

                _messages.Add(ChatMessage.FromAssistant(request.Result));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(request.Result);
                Console.ResetColor();

                stopChat = Prompt.Confirm("Close conversation and copy answer");

                if (stopChat)
                    Clipboard.SetText(request.Result);

                chatCounter++;

            } while (!stopChat);

            return;
        }

        if (args[0] == "-p")
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException($"{_filePath}");

            var pageSize = int.Parse(args[1]);

            var psch = GetPowerShellCommandHistory();

            var selectedCommand = SelectCommand("Filter:", psch, pageSize);

            Clipboard.SetText(selectedCommand);

            ClearPSCH(psch);

            return;
        }

        if (args[0] == "-v")
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;

            Console.WriteLine($"PowerShell Command History {version}");

            return;
        }

        if (args[0] == "-h")
        {
            Console.WriteLine("Usage:  psch [OPTIONS] COMMAND");
            Console.WriteLine("Options:");
            Console.WriteLine("-h       Print help and quit");
            Console.WriteLine("-v       Print version information and quit");
            Console.WriteLine("-p       Set page size");
            Console.WriteLine("-s       Save command");
            Console.WriteLine("-r       Remove command");
            Console.WriteLine("-f       Saved commands list");

            return;
        }

        if (args[0] == "-s")
        {
            if (args.Length > 2)
            {
                Console.WriteLine("Use quote to save multi args command");
                Console.WriteLine("E.g. psch -s \"your multi args command\"");

                return;
            }

            var cmd = new FavouriteCommand
            {
                Command = args[1]
            };

            _dbContext
                .FavouriteCommand
                .Add(cmd);

            _dbContext
                .SaveChanges();

            return;
        }

        if (args[0] == "-r")
        {
            var fCommands = _dbContext
                            .FavouriteCommand
                            .Select(s => s.Command)
                            .ToList();

            var selectCommand = SelectCommand("Remove:", fCommands);

            var favouriteCommand = _dbContext
                 .FavouriteCommand
                 .Where(x => x.Command == selectCommand)
                 .First();

            _dbContext
                .Remove(favouriteCommand);

            _dbContext
                .SaveChanges();

            return;
        }

        if (args[0] == "-f")
        {
            var fCommands = _dbContext
                           .FavouriteCommand
                           .Select(s => s.Command)
                           .ToList();

            var selectedCommand = SelectCommand("Filter:", fCommands);

            Clipboard.SetText(selectedCommand);

            return;
        }

        throw new ArgumentException("Unknown option \nTry 'psch -h' for more information.");
    }

    private static List<string> GetPowerShellCommandHistory()
    {
        return File
            .ReadLines(_filePath)
            .Where(x => !x.StartsWith("psch", StringComparison.InvariantCultureIgnoreCase))
            .Distinct()
            .Reverse()
            .ToList();   
    }

    private static string SelectCommand(string fallbackValue, List<string> commands, int pageSize = 10)
    {
        Prompt.Symbols.Prompt = new Symbol("", fallbackValue);
        Prompt.ColorSchema.PromptSymbol = ConsoleColor.Gray;

        return Prompt
            .Select(string.Empty, commands, pageSize);
    }

    static void ClearPSCH(List<string> commandHistory)
    {
        File.Delete(_filePath);

        using StreamWriter sw = File.AppendText(_filePath);
        commandHistory.ForEach(x => sw.WriteLine(x));
    }
}