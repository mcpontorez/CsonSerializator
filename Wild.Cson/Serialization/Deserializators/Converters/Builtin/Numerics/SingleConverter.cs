using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin.Numerics
{
    public class SingleConverter : IBuiltinTypeConverter
    {
        private const int _charCount = 15 + 1;
        private const char _valueEndCharUpperCase = 'F', _valueEndCharLowerCase = 'f';
        private static readonly char[] _valueEndChars = new char[] { _valueEndCharUpperCase, _valueEndCharLowerCase };

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
            if (cson.TrySkip(CharConsts.Minus))
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

            value *= sign;

            cson.SkipAnyIfNeeds(_valueEndChars);
            return value;
        }
        private static float Convert(char digit) => digit - 48;

        public bool IsConvertable(CsonReader cson)
        {
            char currentChar = cson.CurrentChar;
            if (!(char.IsDigit(currentChar) || currentChar == CharConsts.Minus || currentChar == CharConsts.Dot))
                return false;

            int endIndex = cson.IndexOfAny(_valueEndChars, 1, _charCount);

            return endIndex > -1;
        }

        public bool IsCanConvertable(Type type) => type == typeof(float);
    }
}
