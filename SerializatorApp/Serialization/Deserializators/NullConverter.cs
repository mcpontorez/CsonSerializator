using System;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators
{
    public class NullConverter : IConverter
    {
        private const string _startString = "null";

        public T From<T>(StringReader cson)
        {
            cson.SkipStartsWith(_startString);
            return default;
        }

        public bool IsCanConvertable(StringReader cson)
        {
            if (!cson.StartsWith(_startString))
                return false;
            char postChar = cson[_startString.Length];
            bool result = char.IsSeparator(postChar) || postChar == ';';
            return result;
        }
    }
}
