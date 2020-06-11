using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public class StringConverter : IConverter
    {
        private const char _startChar = '"', _endChar = '"';

        public T From<T>(StringReader cson)
        {
            cson.SkipOne();
            string result = cson.TakeUntil(_endChar);
            cson.SkipOne().SkipWhileSeparators().SkipOne();
            return (T)(object)result; 

        }

        public bool IsCanConvertable(StringReader cson) => cson.StartsWith(_startChar);
    }
}
