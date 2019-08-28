namespace QuicNet.Infrastructure
{
    public class NumberSpace
    {
        private readonly uint _max = uint.MaxValue;

        private uint _n;

        public NumberSpace()
        {
        }

        public NumberSpace(uint max)
        {
            _max = max;
        }

        public bool IsMax()
        {
            return _n == _max;
        }

        public uint Get()
        {
            if (_n >= _max)
                return 0;

            _n++;
            return _n;
        }
    }
}
