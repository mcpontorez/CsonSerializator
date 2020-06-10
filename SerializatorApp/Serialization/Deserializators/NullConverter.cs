using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public class NullConverter : IConverter
    {
        private const string _startsWithValue = "null";

        public T From<T>(StringReader cson)
        {
            cson.SkipStartsWith(_startsWithValue);
            return default;
        }

        public bool IsCanConvertable(StringReader cson) => cson.StartsWith(_startsWithValue);
    }
}
