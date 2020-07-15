using SerializatorApp.Serialization.Utils;
using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public class StringConverter : IConcreteConverter<string>
    {
        private const char _startChar = '"', _endChar = '"';

        public TResult Convert<TResult>(StringReader cson, ITypeNameResolver typeNameResolver) => ConvertToConcrete(cson).Cast<TResult>();

        public string ConvertToConcrete(StringReader cson)
        {
            cson.SkipOne();
            string result = cson.TakeUntil(_endChar);
            cson.SkipOne();
            return result;
        }

        public bool IsCanConvertable(StringReader cson) => cson.StartsWith(_startChar);

        public bool IsCanConvertable(Type type) => type == typeof(string);
    }
}
