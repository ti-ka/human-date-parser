using System;
using System.Text;
using HumanDateParser;

namespace HumanDateParser
{
    public class Lexer
    {
        /// <summary>
        /// Exposed for unit testing
        /// </summary>
        public ICharacterBuffer _buffer;

        public Lexer(ICharacterBuffer charBuffer)
        {
            _buffer = charBuffer;
        }

        /// <summary>
        /// Gets the next Token
        /// </summary>
        /// <returns></returns>
        public Token GetNextToken()
        {
            Token returnToken = null;
            while (true)
            {
                switch (_buffer.Peek(1))
                {
                    case ' ':
                        _buffer.Load();
                        break;
                    case -1:
                        returnToken = new Token(TokenKind.EOF, "<eof>");
                        break;
                    case '-':
                        returnToken = new Token(TokenKind.TO, "<to>");
                        _buffer.Load();
                        break;
                    case ':':
                        returnToken = new Token(TokenKind.COLON, "<:>");
                        _buffer.Load();
                        break;
                       
                    default:
                        if (char.IsLetter((char)_buffer.Peek(1)))
                        {
                            returnToken = GetIdentifier();
                        }
                        else if (char.IsNumber((char)_buffer.Peek(1)))
                        {
                            if (_buffer.Peek(2) == '-' || _buffer.Peek(2) == '/' ||
                                _buffer.Peek(3) == '-' || _buffer.Peek(3) == '/')
                            {
                                return new Token(TokenKind.DATE_IDENTIFIER, ReadIdentifierToEnd());
                            }
           
                            return GetNumber();
                        }
                        else
                        {
                            _buffer.Load();
                        }
                        break;
                }

                if (returnToken != null) return returnToken;
            }
        }

        /// <summary>
        /// Get keyword identifier/datetime variable
        /// </summary>
        /// <returns></returns>
        private Token GetIdentifier()
        {
            Token returnToken = null;
            var identifier = ReadIdentifierToEnd();
            switch (identifier.ToUpper())
            {
                case "TODAY":
                    returnToken = new Token(TokenKind.TODAY, "<today>");
                    break;
                case "TOMORROW":
                    returnToken = new Token(TokenKind.TOMORROW, "<tomorrow>");
                    break;
                case "YESTERDAY":
                    returnToken = new Token(TokenKind.YESTERDAY, "<yesterday>");
                    break;
                case "JAN":
                case "JANUARY":
                case "FEB":
                case "FEBUARY":
                case "MAR":
                case "MARCH":
                case "APR":
                case "APRIL":
                case "MAY":
                case "JUN":
                case "JUNE":
                case "JUL":
                case "JULY":
                case "AUG":
                case "AUGUST":
                case "SEPT":
                case "SEP":
                case "SEPTEMBER":
                case "OCT":
                case "OCTOBER":
                case "NOV":
                case "NOVEMBER":
                case "DEC":
                case "DECEMBER":
                    returnToken = new Token(TokenKind.MONTH_IDENTIFIER, identifier.ToUpper());
                    break;
                case "MONDAY":
                case "TUESDAY":
                case "WEDNESDAY":
                case "THURSDAY":
                case "FRIDAY":
                case "SATURDAY":
                case "SUNDAY":
                    returnToken = new Token(TokenKind.DAY_IDENTIFIER, identifier.ToUpper());
                    break;
                case "YEAR":
                case "YEARS":
                    returnToken = new Token(TokenKind.YEAR, "<year>");
                    break;
                case "MONTH":
                case "MONTHS":
                    returnToken = new Token(TokenKind.MONTH, "<month>");
                    break;
                case "WEEK":
                case "WEEKS":
                    returnToken = new Token(TokenKind.WEEK, "<week>");
                    break;
                case "DAY":
                case "DAYS":
                    returnToken = new Token(TokenKind.DAY, "<day>");
                    break;
                case "NEXT":
                    returnToken = new Token(TokenKind.NEXT, "<next>");
                    break;
                case "PREVIOUS":
                    returnToken = new Token(TokenKind.PREVIOUS, "<previous>");
                    break;
                case "AT":
                    returnToken = new Token(TokenKind.AT, "<at>");
                    break;
                case "TO":
                    returnToken = new Token(TokenKind.TO, "<to>");
                    break;
                case "AGO":
                    returnToken = new Token(TokenKind.AGO, "<ago>");
                    break;
                case "TH":
                case "RD":
                case "ND":
                case "ST":
                    returnToken = new Token(TokenKind.MONTH_MODIFIER, "<month_modifier>");
                    break;
                case "AM":
                case "PM":
                    returnToken = new Token(TokenKind.TIME_MODIFIER, identifier.ToUpper());
                    break;
                case "END":
                    returnToken = new Token(TokenKind.EOF, "<eof>");
                    break;
                   
            }
            return returnToken;
        }
        /// <summary>
        /// Read Identifier to the end
        /// </summary>
        /// <returns></returns>
        private string ReadIdentifierToEnd()
        {
            var s = new StringBuilder();
            while (char.IsLetter((char)_buffer.Peek(1)) || char.IsNumber((char)_buffer.Peek(1)) || (char)_buffer.Peek(1) == '_' || (char)_buffer.Peek(1) == '/' || (char)_buffer.Peek(1) == '-' || (char)_buffer.Peek(1) == '.')
            {
                s.Append((char)_buffer.Peek(1));
                _buffer.Load();
            }
            return s.ToString();
        }

        /// <summary>
        /// Read the full number
        /// </summary>
        /// <returns></returns>
        private Token GetNumber()
        {
            var s = new StringBuilder();
            var c = (char)_buffer.Peek(1);
            while (char.IsNumber(c) )
            {
                s.Append(c);
                _buffer.Load();
                c = (char)_buffer.Peek(1);
            }
            var stemp = s.ToString();
            return new Token(TokenKind.NUMBER, stemp);
        }
    }
}