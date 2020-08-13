using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Numerics
{
    public class Int32Converter : ConverterBase, IConcreteConverter<int>
    {
        public override TResult Convert<TResult>(CsonReader cson, ITypeNameResolver typeNameResolver) => ConvertToConcrete(cson).Cast<TResult>();

        public int ConvertToConcrete(CsonReader cson)
        {
            string value = cson.TakeWhile(c => char.IsDigit(c) || c == '-');
            return int.Parse(value);
        }

        public override bool IsCanConvertable(CsonReader cson)
        {
            if (!(char.IsDigit(cson.CurrentChar) || cson.CurrentChar == '-'))
                return false;
            int endIndex = cson.IndexOfAny(_endChars);
            if (endIndex > 12)
                return false;
            for (int i = 1; i < endIndex; i++)
            {
                if (!char.IsDigit(cson[i]))
                    return false;
            }
            return true;
        }

        public bool IsCanConvertable(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
