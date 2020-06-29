using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Deserializators.Numerics
{
    public class SingleConverter : IConverter
    {
        private const char _endCharUpperCase = 'F', _endCharLowerCase = 'f';
        private readonly IReadOnlyList<char> _endChars = new char[] { _endCharUpperCase, _endCharLowerCase };

        public T From<T>(StringReader cson)
        {
            string value = cson.TakeWhile(c => char.IsDigit(c) || c == '-' || c == '.');

            T result = (T)(object)float.Parse(value);
            cson.SkipOne();
            return result;
        }

        public bool IsCanConvertable(StringReader cson)
        {
            if (!(char.IsDigit(cson.CurrentChar) || cson.CurrentChar == '-'))
                return false;

            int endIndex = cson.IndexOfAny(_endChars);

            for (int i = 0; i < endIndex; i++)
            {
                if (!char.IsDigit(cson[i]))
                    return false;
            }
            return true;
        }
    }
}
