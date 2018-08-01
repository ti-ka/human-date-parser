using System;

namespace HumanDateParser
{
    public class CharacterBuffer : ICharacterBuffer
    {
        private int _currentPosition;
        private readonly int[] _bufferArray;
        private readonly string _code;
        private readonly int _size;

        public CharacterBuffer(string script, int bufferSize)
        {
            _code = script;
            _size = bufferSize;
            _bufferArray = new int[_size];
            SetPos(0);
        }

        public void SetPos(int position)
        {
            _currentPosition = position;
            // bufferArray[size - 1] = code[currentPosition++];
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Load next set of charactors into the buffer
        /// </summary>
        public void Load(int length)
        {
            if(length > _size) length = _size;
            for (var i = 1; i <= length; i++)
                Load();
        }

        /// <inheritdoc />
        /// <summary>
        /// Load next charactor into the buffer
        /// </summary>
        public void Load()
        {
            for (var i = 0; i < _size - 1; i++){
                _bufferArray[i] = _bufferArray[i + 1];
            }
            try{
                if (_currentPosition == _code.Length){
                    _bufferArray[_size - 1] = -1;
                }
                else{
                    _bufferArray[_size - 1] = _code[_currentPosition++];
                }
            }
            catch (Exception e){
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Peeks at the charactor(s) from the buffer
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public int Peek(int pos)
        {
            if (pos >= 1 && pos <= _size)
                return _bufferArray[pos - 1];
            return 0;
        }
    }
}