using System;
using HumanDateParser;

namespace HumanDateParser
{
    public class TokenBuffer
    {
        private readonly Token[] _buffer;
        private readonly int _size;
        private readonly Lexer _lexer;


        public TokenBuffer(ICharacterBuffer characterBuffer, int bufferSize)
            : this(new Lexer(characterBuffer), bufferSize)
        {  
        }

        public TokenBuffer(Lexer lex, int bufferSize)
        {
            _size = bufferSize;
            _buffer = new Token[bufferSize];
            _lexer = lex;
            InitBuffer(bufferSize);
        }

        private void InitBuffer(int bufferSize)
        {
            try{
                for (var i = 0; i < bufferSize; i++)
                    _buffer[i] = _lexer.GetNextToken();
            }
            catch (Exception e){
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Peek at position in token buffer
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Token Peek(int pos)
        {
            if (pos >= 1 && pos <= _size)
                return _buffer[pos - 1];

            return null;
        }

        /// <summary>
        /// Load X tokens into the buffer
        /// </summary>
        public void Load(int length)
        {
            if (length > _size) length = _size;
            for (var i = 1; i <= length; i++)
                Load();
        }

        /// <summary>
        /// Load next token
        /// </summary>
        public void Load()
        {
            for (var i = 0; i < _size - 1; i++)
                _buffer[i] = _buffer[i + 1];

            try{
                _buffer[_size - 1] = _lexer.GetNextToken();
            }
            catch (Exception e){
                Console.WriteLine(e.Message);
            }
        }
    }
}