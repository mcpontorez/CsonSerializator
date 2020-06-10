using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public class NullConverter : IConverter
    {
        private const string _startString = "null;";

        public T From<T>(StringReader cson)
        {
            cson.SkipStartsWith(_startString);
            return default;
        }

        public bool IsCanConvertable(StringReader cson) => cson.StartsWith(_startString);
    }
}
