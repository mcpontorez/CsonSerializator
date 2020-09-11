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

        public TResult Convert<TResult>(CsonReader cson) => UltraConvertToConcrete(cson).WildCast<TResult>();

        public float ConvertToConcrete(CsonReader cson)
        {
            string value = cson.TakeWhile(c => char.IsDigit(c) || c == CharConsts.Minus || c == CharConsts.Dot);
            float result = float.Parse(value, CultureInfo.InvariantCulture);
            cson.SkipOne();
            return result;
        }

        private static float UltraConvertToConcrete(CsonReader cson)
        {
            float sign = 1F, value = 0F;
            if (cson.CurrentChar == CharConsts.Minus)
            {
                sign = -1F;
                cson.SkipWhileSeparators();
            }
            char currentChar;
            while (cson.IsNotEnded && char.IsDigit(currentChar = cson.CurrentChar))
            {
                value *= 10F;
                value += Convert(currentChar);
                cson.AddIndex();
            }
            value *= sign;

            if(cson.TrySkip(CharConsts.Dot))
            {
                float multiplier = 1F;
                while (cson.IsNotEnded && char.IsDigit(currentChar = cson.CurrentChar))
                {
                    multiplier *= 0.1F;
                    value += Convert(currentChar) * multiplier;
                    cson.AddIndex();
                }
            }

            if (cson.IsNotEnded && (cson.CurrentChar == _valueEndCharUpperCase || cson.CurrentChar == _valueEndCharLowerCase))
                cson.SkipOne();
            return value;
        }
        private static float Convert(char digit) => digit - 48;

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
