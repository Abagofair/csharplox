namespace csharplox;

public static class Lox
{
    private static bool HadError = true;

    public static void RunFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

        string source = File.ReadAllText(path);
        Run(source);

        if (HadError)
        {
            Console.WriteLine("Exiting because of an unexpected error");
        }
    }

    public static void RunPrompt()
    {
        StreamReader reader = new StreamReader(Console.OpenStandardInput());

        while (true)
        {
            Console.Write("> ");
            string? line = reader.ReadLine();

            if (line == null || line == "exit")
            {
                break;
            }

            if (line == "help")
            {
                Console.WriteLine("No help from here.. YET!");
            }

            Run(line);

            HadError = false;
        }
    }

    public static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = new List<Token>();

        foreach (Token token in tokens)
        {
            Console.WriteLine(token.ToString());
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    public static void Report(int line, string where, string message)
    {
        Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
        HadError = true;
    }
}