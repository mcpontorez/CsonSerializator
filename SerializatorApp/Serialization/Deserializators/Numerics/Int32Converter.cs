using System;

namespace SerializatorApp.Serialization.Deserializators.Numerics
{
    public class Int32Converter : IConverter
    {
        private const char _endChar = ';';

        public T From<T>(StringReader cson)
        {
            T result = (T)(object)int.Parse(cson.TakeUntil(_endChar));
            cson.SkipWhileSeparators().SkipOne();
            return result;
        }

        public bool IsCanConvertable(StringReader cson)
        {
            int endIndex = cson.IndexOf(_endChar);
            for (int i = 0; i < endIndex; i++)
            {
                if (!char.IsDigit(cson[i]))
                    return false;
            }
            return true;
        }
    }
}
