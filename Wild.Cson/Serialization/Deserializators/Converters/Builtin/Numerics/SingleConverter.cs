using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin.Numerics
{
    public class SingleConverter : IBuiltinTypeConverter
    {
        private const char _valueEndCharUpperCase = 'F', _valueEndCharLowerCase = 'f';
        private static readonly IReadOnlyList<char> _valueEndChars = new char[] { _valueEndCharUpperCase, _valueEndCharLowerCase };

        public TResult Convert<TResult>(CsonReader cson) => ConvertToConcrete(cson).WildCast<TResult>();

        public float ConvertToConcrete(CsonReader cson)
        {
            string value = cson.TakeWhile(c => char.IsDigit(c) || c == CharConsts.Minus || c == CharConsts.Dot);
            float result = float.Parse(value, CultureInfo.InvariantCulture);
            cson.SkipOne();
            return result;
        }

        public bool IsConvertable(CsonReader cson)
        {
            char currentChar = cson.CurrentChar;
            if (!(char.IsDigit(currentChar) || currentChar == CharConsts.Minus || currentChar == CharConsts.Dot))
                return false;

            int endIndex = cson.IndexOfAny(_valueEndChars);

            for (int i = 1; i < endIndex; i++)
            {
                char c = cson[i];
                if (!(char.IsDigit(c) || c == CharConsts.Dot))
                    return false;
            }
            return true;
        }

        public bool IsCanConvertable(Type type) => type == typeof(float);
    }
}
