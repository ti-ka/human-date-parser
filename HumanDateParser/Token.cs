namespace HumanDateParser
{
    public class Token
    {
        public string Text { get; set; }
        public TokenKind Kind { get; set; }

        public Token(TokenKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }
    }
}