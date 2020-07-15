using SerializatorApp.Serialization.Utils;
using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Deserializators.Numerics
{
    public class SingleConverter : IConcreteConverter<float>
    {
        private const char _endCharUpperCase = 'F', _endCharLowerCase = 'f';
        private readonly IReadOnlyList<char> _endChars = new char[] { _endCharUpperCase, _endCharLowerCase };

        public TResult Convert<TResult>(StringReader cson, ITypeNameResolver typeNameResolver) => ConvertToConcrete(cson).Cast<TResult>();

        public float ConvertToConcrete(StringReader cson)
        {
            string value = cson.TakeWhile(c => char.IsDigit(c) || c == '-' || c == '.');
            float result = float.Parse(value);
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

        public bool IsCanConvertable(Type type) => type == typeof(float);
    }
}
