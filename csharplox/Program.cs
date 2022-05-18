// See https://aka.ms/new-console-template for more information
using csharplox;

if (args.Length > 1)
{
    Console.WriteLine("Usage: jlox [scipt]");
}
else if (args.Length == 1)
{
    Lox.RunFile(args[0]);
}
else
{
    Lox.RunPrompt();
}