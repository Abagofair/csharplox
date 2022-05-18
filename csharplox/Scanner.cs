namespace csharplox;

public class Scanner
{
    private readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
    {
        {"and", TokenType.AND},
        {"class", TokenType.CLASS},
        {"else", TokenType.ELSE},
        {"false", TokenType.FALSE},
        {"for", TokenType.FOR},
        {"fun", TokenType.FUN},
        {"if", TokenType.IF},
        {"nil", TokenType.NIL},
        {"or", TokenType.OR},
        {"print", TokenType.PRINT},
        {"return", TokenType.RETURN},
        {"super", TokenType.SUPER},
        {"this", TokenType.THIS},
        {"true", TokenType.TRUE},
        {"var", TokenType.VAR},
        {"while", TokenType.WHILE}
    };

    private readonly string _source;
    private readonly List<Token> _tokens = new();

    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    public Scanner(string source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.EOF, "", null, _line));
        return _tokens;
    }

    private void ScanToken()
    {
        char c = Advance();

        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;

            case '!':
                AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;

            case '/':
                if (Match('/'))
                {
                    while (Peek() != '\n' && !IsAtEnd())
                        Advance();
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;

            case '"':
                String();
                break;

            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace.
                break;

            case '\n':
                _line++;
                break;

            default:
                if (IsDigit(c))
                {
                    Number();
                }
                else if (IsAlpha(c))
                {
                    Identifier();
                }
                else
                {
                    Lox.Error(_line, "Unexpected character");
                }
                break;
        }
    }

    private void Number()
    {
        AdvanceWhileDigit();

        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            Advance();

            AdvanceWhileDigit();
        }

        AddToken(TokenType.NUMBER, double.Parse(_source.Substring(_start, _current)));

        void AdvanceWhileDigit()
        {
            while (IsDigit(Peek()))
                Advance();
        }
    }

    private bool IsDigit(char c) => c >= '0' && c <= '9';

    private bool IsAlpha(char c)
        => (c >= 'a' && c <= 'z') ||
           (c >= 'A' && c <= 'Z') ||
           (c == '_');

    private bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek()))
            Advance();

        string text = _source.Substring(_start, _current);
        
        if (_keywords.TryGetValue(text, out TokenType type))
        {
            AddToken(type);
        }
        else
        {
            AddToken(TokenType.IDENTIFIER);
        }
    }

    private void String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n')
                _line++;

            Advance();
        }

        if (IsAtEnd())
        {
            Lox.Error(_line, "Unterminated string");
            return;
        }

        Advance();

        string value = _source.Substring(_start + 1, _current - 1);
        AddToken(TokenType.STRING, value);
    }

    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return _source[_current];
    }

    private char PeekNext()
    {
        if (_current + 1 >= _source.Length) return '\0';
        return _source[_current + 1];
    }

    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (_source[_current] != expected) return false;

        _current++;

        return true;
    }

    private char Advance() => _source[_current++];

    private void AddToken(TokenType tokenType)
    {
        AddToken(tokenType, null);
    }

    private void AddToken(TokenType tokenType, object? literal)
    {
        string text = _source.Substring(_start, _current);
        _tokens.Add(new Token(tokenType, text, literal, _line));
    }

    private bool IsAtEnd() => _current >= _source.Length;
}