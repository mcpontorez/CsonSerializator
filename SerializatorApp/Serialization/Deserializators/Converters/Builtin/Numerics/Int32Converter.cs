using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Builtin.Numerics
{
    public class Int32Converter : IBuiltinTypeConverter
    {
        public TResult Convert<TResult>(CsonReader cson) => ConvertToConcrete(cson).Cast<TResult>();

        public int ConvertToConcrete(CsonReader cson)
        {
            string value = cson.TakeWhile(c => char.IsDigit(c) || c == '-');
            return int.Parse(value);
        }

        public bool IsCanConvertable(CsonReader cson)
        {
            char currentChar = cson.CurrentChar;
            if (!(char.IsDigit(currentChar) || currentChar == CharConsts.Minus))
                return false;
            int endIndex = cson.IndexOfAny(CharConsts.e);
            if (endIndex > 12)
                return false;
            for (int i = 1; i < endIndex; i++)
            {
                if (!char.IsDigit(cson[i]))
                    return false;
            }
            return true;
        }
    }
}
