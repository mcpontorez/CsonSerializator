using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin.Numerics
{
    public class Int32Converter : IBuiltinTypeConverter
    {
        private const int minValueLenght = 11, maxValueLenght = 10;

        public TResult Convert<TResult>(CsonReader cson) => ConvertToConcrete(cson).WildCast<TResult>();

        public int ConvertToConcrete(CsonReader cson)
        {
            string value = cson.TakeWhile(c => char.IsDigit(c) || c == CharConsts.Minus);
            return int.Parse(value);
        }

        public bool IsConvertable(CsonReader cson)
        {
            char currentChar = cson.CurrentChar;
            if (!(char.IsDigit(currentChar) || currentChar == CharConsts.Minus))
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
    }
}
