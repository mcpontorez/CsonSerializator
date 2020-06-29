using System;

namespace SerializatorApp.Serialization.Deserializators.Numerics
{
    public class Int32Converter : ConverterBase
    {
        public override T From<T>(StringReader cson)
        {
            string value = cson.TakeWhile(c => char.IsDigit(c) || c == '-');
            T result = (T)(object)int.Parse(value);
            return result;
        }

        public override bool IsCanConvertable(StringReader cson)
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
    }
}
