using Sharprompt;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

class Program
{
    private readonly static string _filePath = Environment.ExpandEnvironmentVariables(@"%userprofile%\AppData\Roaming\Microsoft\Windows\PowerShell\PSReadline\ConsoleHost_history.txt");
    private static int pageSize = 10;

    [DllImport("user32.dll")]
    internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll")]
    internal static extern bool CloseClipboard();

    [DllImport("user32.dll")]
    internal static extern bool SetClipboardData(uint uFormat, IntPtr data);

    [STAThread]
    private static void Main(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals("-v") || args[i].Equals("-h") || args[i].Equals("-p") && IsEven(i))
            {

                if (args[i].Equals("-v"))
                {
                    string currentVerion = GetAssemblyFileVersion();
                    Console.WriteLine($"PowerShell Command History {currentVerion}");

                    return;
                }

                if (args[i].Equals("-h"))
                {
                    Console.WriteLine("Usage:  psch [OPTIONS] COMMAND");
                    Console.WriteLine("Options:");
                    Console.WriteLine("-h       Print help and quit");
                    Console.WriteLine("-v       Print version information and quit");
                    Console.WriteLine("-p       Set page size");

                    return;
                }

                if (args[i].Equals("-p"))
                {
                    pageSize = Int32.Parse(args[i + 1]);
                }
            }
            else if (IsOdd(i))
            {
                continue;
            }
            else
            {
                throw new ArgumentException("Unknown option \nTry 'psch -h' for more information.");
            }
        }

        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"{_filePath}");

        var commandHistory = File.ReadAllLines(_filePath)
                .Reverse()
                .Distinct();

        Prompt.Symbols.Prompt = new Symbol("", "Filter:");
        Prompt.ColorSchema.PromptSymbol = ConsoleColor.Gray;

        var command = Prompt
            .Select(string.Empty, commandHistory, pageSize);

        CopyToClipboard(command);

        ClearPSCH(commandHistory);
    }

    static void CopyToClipboard(string command)
    {
        OpenClipboard(IntPtr.Zero);
        var ptr = Marshal.StringToHGlobalUni(command);
        SetClipboardData(13, ptr);
        CloseClipboard();
        Marshal.FreeHGlobal(ptr);
    }

    static string GetAssemblyFileVersion()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
        return string.IsNullOrEmpty(fileVersion.FileVersion) ? "Cannot find version" : fileVersion.FileVersion;
    }

    static bool IsEven(int value)
    {
        return value % 2 == 0;
    }

    static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    static void ClearPSCH(IEnumerable<string> commandHistory)
    {
        File.Delete(_filePath);

        using StreamWriter sw = File.AppendText(_filePath);
        foreach (var command in commandHistory)
        {
            if (command.StartsWith("psch", StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }
            else
            {
                sw.WriteLine(command);
            }
        }
    }
}