using System;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators
{
    public class NullConverter : ConverterBase
    {
        private const string _startString = "null";

        public override T From<T>(StringReader cson)
        {
            cson.SkipStartsWith(_startString);
            return default;
        }

        public override bool IsCanConvertable(StringReader cson)
        {
            if (!cson.StartsWith(_startString))
                return false;
            char postChar = cson[_startString.Length];
            bool result = char.IsSeparator(postChar) || char.IsControl(postChar) || IsAnyEndChar(postChar);
            return result;
        }
    }
}
