namespace csharplox;

public class Token
{
    public Token(
        TokenType type,
        string lexeme,
        object? literal,
        int line)
    {
        if (string.IsNullOrWhiteSpace(lexeme)) throw new ArgumentException(nameof(lexeme));

        Type = type;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;
    }

    public TokenType Type { get; init; }
    public string Lexeme { get; init; }
    public object? Literal { get; init; }
    public int Line { get; init; }
}