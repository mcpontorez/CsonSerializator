using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin.Numerics
{
    public class Int32Converter : IBuiltinTypeConverter
    {
        private const int minValueLenght = 11, maxValueLenght = 10;
        private static Func<char, bool> IsConvertableFunc = IsConvertable;

        private static bool IsConvertable(char c) => char.IsDigit(c) || c == CharConsts.Minus;
        public bool IsConvertable(ICsonReader cson)
        {
            char currentChar = cson.CurrentChar;
            if (!(IsConvertable(currentChar)))
                return false;

            int count = cson.GetTrueLenght(minValueLenght);
            for (int i = 1; i < count; i++)
            {
                char c = cson[i];
                if (!char.IsDigit(c))
                    return c.IsSeparatorOrAnyEndChar();
            }
            return true;
        }

        public TResult Convert<TResult>(ICsonReader cson) => UltraConvertToConcrete(cson).WildCast<TResult>();

        public int ConvertToConcrete(ICsonReader cson)
        {
            string value = cson.TakeWhile(IsConvertableFunc);
            return int.Parse(value);
        }

        private static int UltraConvertToConcrete(ICsonReader cson)
        {
            int sign = 1, value = 0;
            if (cson.CurrentChar == CharConsts.Minus)
            {
                sign = -1;
                cson.SkipWhileSeparators();
            }
            char currentChar;
            while (cson.IsNotEnded && char.IsDigit(currentChar = cson.CurrentChar))
            {
                value *= 10;
                value += Convert(currentChar);
                cson.AddIndex();
            }
            value *= sign;

            return value;
        }
        private static int Convert(char digit) => digit - 48;
    }
}
