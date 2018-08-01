namespace HumanDateParser
{
    public interface ICharacterBuffer
    {
        void SetPos(int position);

        /// <summary>
        /// Load next set of charactors into the buffer
        /// </summary>
        void Load(int length);

        /// <summary>
        /// Load next charactor into the buffer
        /// </summary>
        void Load();

        /// <summary>
        /// Peeks at the charactor(s) from the buffer
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        int Peek(int pos);
    }
}