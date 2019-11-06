namespace HumanDateParser
{
    public enum TokenKind
    {
        YEAR,
        MONTH,
        WEEK,
        DAY,
        MONTH_IDENTIFIER,
        MONTH_MODIFIER,
        TIME_MODIFIER,
        DAY_IDENTIFIER,
        TODAY,
        TOMORROW,
        YESTERDAY,
        DATE_IDENTIFIER,
        NUMBER,
        NEXT,
        PREVIOUS,
        TO,
        AT,
        AGO,
        COLON,
        EOF
    }
}