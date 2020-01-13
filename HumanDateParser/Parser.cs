using System;
using System.Collections.Generic;

namespace HumanDateParser
{
    public class Parser
    {
        public List<string> Errors = new List<string>();
        private readonly Dictionary<string, int> _months = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _days = new Dictionary<string, int>();

        private const int LookaheadDepth = 2;
        private readonly TokenBuffer _input;
        private DateRange _dateRange = null;

        public Parser(Lexer lex)
        {
            Init();
            _input = new TokenBuffer(lex, LookaheadDepth);
        }

        public Parser(TokenBuffer t)
        {
            Init();
            _input = t;
        }

        private void Init()
        {
            _months.Add("JAN", 1);
            _months.Add("FEB", 2);
            _months.Add("MAR", 3);
            _months.Add("APR", 4);
            _months.Add("MAY", 5);
            _months.Add("JUN", 6);
            _months.Add("JUL", 7);
            _months.Add("AUG", 8);
            _months.Add("SEPT", 9);
            _months.Add("OCT", 10);
            _months.Add("NOV", 11);
            _months.Add("DEC", 12);

            _months.Add("JANUARY", 1);
            _months.Add("FEBUARY", 2);
            _months.Add("MARCH", 3);
            _months.Add("APRIL", 4);
            _months.Add("JUNE", 6);
            _months.Add("JULY", 7);
            _months.Add("AUGUST", 8);
            _months.Add("SEPTEMBER", 9);
            _months.Add("OCTOBER", 10);
            _months.Add("NOVEMBER", 11);
            _months.Add("DECEMBER", 12);

        }

        public DateRange Eval()
        {
            _dateRange = new DateRange();
            DateExpression();

            while (Peek(1).Kind != TokenKind.EOF){
                if (Peek(1).Kind == TokenKind.TO) Load();
                DateExpression();
            }
            return _dateRange;
        }

        private void DateExpression()
        {
            switch (Peek(1).Kind)
            {
                case TokenKind.TODAY:
                    _dateRange.AddDate(Today);
                    Load();
                    break;
                case TokenKind.TOMORROW:
                    _dateRange.AddDate(Today.AddDays(1));
                    Load();
                    break;
                case TokenKind.YESTERDAY:
                    _dateRange.AddDate(Today.AddDays(-1));
                    Load();
                    break;
                case TokenKind.DAY_IDENTIFIER:
                    DayDateIdent(Peek(1).Text, TokenKind.DAY);
                    Load();
                    break;
                case TokenKind.NUMBER:
                    var num = int.Parse(Peek(1).Text);
                    Load();
                    switch(Peek(1).Kind)
                    {
                        case TokenKind.DAY:
                        case TokenKind.WEEK:
                        case TokenKind.MONTH:
                        case TokenKind.YEAR:
                            NumQuickDateIdent(num);
                            break;
                        case TokenKind.MONTH_MODIFIER:
                            Load();
                            MonthDateIdent(num);
                            break;
                    }
                    break;
                case TokenKind.NEXT:
                    Load();
                    NumQuickDateIdent(1);
                    break;
                case TokenKind.PREVIOUS:
                    Load();
                    NumQuickDateIdent(-1);
                    break;
            }

            //Get Or Time Year
            switch (Peek(1).Kind)
            {
                case TokenKind.NUMBER:
                    Year();
                    Load();
                    if (Peek(1).Kind == TokenKind.AT)
                        Time();
                    break;
                case TokenKind.AT:
                    Time();
                    break;
            }

            Load();
        }

        private void Time()
        {
            Load();
            if (Peek(1).Kind == TokenKind.NUMBER)
            {
                var hourPart = int.Parse(Peek(1).Text);
                var minPart = 0;
                Load();

                if (Peek(1).Kind == TokenKind.COLON)
                {
                    Load();
                    if(Peek(1).Kind == TokenKind.NUMBER)
                    {
                        minPart = int.Parse(Peek(1).Text);
                        Load();
                    }
                    else
                    {
                        Errors.Add("Minute time part required after ':' keyword");
                        return; 
                    }
                }

                if (Peek(1).Kind == TokenKind.TIME_MODIFIER && Peek(1).Text == "PM" && hourPart <= 12) hourPart = hourPart + 12;


                _dateRange.CurrentDate = _dateRange.CurrentDate.SetTime(hourPart, minPart);
            }
            else
            {
                Errors.Add("Time required after 'At' keyword");
                return;
            }
        }

        private void Year()
        {
            _dateRange.CurrentDate = (new DateTime(int.Parse(Peek(1).Text), _dateRange.CurrentDate.Month, _dateRange.CurrentDate.Day));
        }

        private void MonthDateIdent(int num)
        {
            if (Peek(1).Kind == TokenKind.MONTH_IDENTIFIER)
            {
                _dateRange.AddDate(new DateTime(DateTime.Now.Year, _months[Peek(1).Text], num));
                Load();
            }
            else
                _dateRange.AddDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, num));
        }

        private void DayDateIdent(string dayString, TokenKind tokenKind)
        {
            DateTime pivotDate;
            if (tokenKind == TokenKind.NEXT)
            {
                pivotDate = Today.AddDays(1);
            }
            else
            {
                pivotDate = Today;
            }

            for (var i = 0; i < 7; i++ )
            {
                if (pivotDate.AddDays(i).DayOfWeek == (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayString, true))
                {
                    _dateRange.AddDate(pivotDate.AddDays(i));
                    return;
                }
            }
            Errors.Add($"Unable to parse day '{dayString}'");
        }

        private void NumQuickDateIdent(int num)
        {
            if (Peek(2).Kind == TokenKind.AGO) num = num * -1;
            switch (Peek(1).Kind)
            {
                case TokenKind.DAY_IDENTIFIER:
                    DayDateIdent(Peek(1).Text, TokenKind.NEXT);
                    Load();
                    break;
                case TokenKind.DAY:
                    _dateRange.AddDate(Today.AddDays(num));
                    Load();
                    break;
                case TokenKind.WEEK:
                    _dateRange.AddDate(Today.AddDays(num * 7));
                    Load();
                    break;
                case TokenKind.MONTH:
                    _dateRange.AddDate(Today.AddMonths(num));
                    Load();
                    break;
                case TokenKind.YEAR:
                    _dateRange.AddDate(Today.AddYears(num));
                    Load();
                    break;
            }
        }

        private static DateTime Today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        #region TokenBufferWrapper
        private void Load(int length)
        {
            _input.Load(length);
        }

        private void Load()
        {
            _input.Load();
        }

        private Token Peek(int pos)
        {
            return _input.Peek(pos);
        }
        #endregion
    }
}