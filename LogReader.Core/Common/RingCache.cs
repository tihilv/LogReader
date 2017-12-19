using System;

namespace LogReader
{
    public class RingCache<T>
    {
        private readonly long _size;
        readonly Func<long, T> _valueProvider;

        readonly T[] _cache;
        readonly bool[] _cacheValidity;
        private long _firstPosition; // position of the first element in the cache array
        private long _firstIndex; // index of the firstElement

        public RingCache(long size, Func<long, T> valueProvider)
        {
            _size = size;
            _valueProvider = valueProvider;

            _cache = new T[_size];
            _cacheValidity = new bool[_size];
        }

        public T this[long index]
        {
            get
            {
                if (index > (_firstIndex + _size - 1))
                {
                    for (long i = _firstIndex; i <= Math.Min(index - _size, _firstIndex + _size); i++)
                        _cacheValidity[GetPositionInCache(i)] = false;
                    var movements = index - _firstIndex - _size + 1;
                    _firstPosition = (_firstPosition + movements)%_size;
                    _firstIndex += movements;
                }

                if (index < _firstIndex)
                {
                    for (long i = _firstIndex; i > Math.Max(index, _firstIndex - _size); i--)
                        _cacheValidity[GetPositionInCache(i-1)] = false;
                    var movements = _firstIndex - index;
                    _firstPosition = (_firstPosition - movements) % _size;
                    _firstIndex -= movements;
                }

                var indexInCache = GetPositionInCache(index);
                if (!_cacheValidity[indexInCache])
                {
                    _cache[indexInCache] = _valueProvider(index);
                    _cacheValidity[indexInCache] = true;
                }

                return _cache[indexInCache];
            }
        }

        long GetPositionInCache(long index)
        {
            long indexInCache = index - _firstIndex + _firstPosition;
            if (indexInCache >= _size)
                indexInCache -= _size;
            if (indexInCache < 0)
                indexInCache += _size;

            return indexInCache;
        }

        public void Clear()
        {
            for (int i = 0; i < _size; i++)
                _cacheValidity[i] = false;
            _firstIndex = 0;
            _firstPosition = 0;
        }
    }
}
