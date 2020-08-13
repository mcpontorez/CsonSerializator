using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SerializatorApp.Serialization.Deserializators.Converters.Numerics
{
    public class SingleConverter : ConverterBase, IConcreteConverter<float>
    {
        private const char _valueEndCharUpperCase = 'F', _valueEndCharLowerCase = 'f';
        private static readonly IReadOnlyList<char> _valueEndChars = new char[] { _valueEndCharUpperCase, _valueEndCharLowerCase };

        public override TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver) => ConvertToConcrete(cson).Cast<TResult>();

        public float ConvertToConcrete(CsonReader cson)
        {
            string value = cson.TakeWhile(c => char.IsDigit(c) || c == '-' || c == '.');
            float result = float.Parse(value, CultureInfo.InvariantCulture);
            cson.SkipOne();
            return result;
        }

        public override bool IsCanConvertable(CsonReader cson)
        {
            if (!(char.IsDigit(cson.CurrentChar) || cson.CurrentChar == '-'))
                return false;

            int endIndex = cson.IndexOfAny(_valueEndChars);

            for (int i = 1; i < endIndex; i++)
            {
                char c = cson[i];
                if (!(char.IsDigit(c) || c == '.'))
                    return false;
            }
            return true;
        }

        public bool IsCanConvertable(Type type) => type == typeof(float);
    }
}
