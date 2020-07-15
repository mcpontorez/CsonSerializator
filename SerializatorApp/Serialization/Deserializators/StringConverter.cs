using SerializatorApp.Serialization.Utils;
using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public class StringConverter : ConverterBase, IConcreteConverter<string>
    {
        private const char _valueStartChar = '"', _valueEndChar = '"';

        public override TResult Convert<TResult>(StringReader cson, ITypeNameResolver typeNameResolver) => ConvertToConcrete(cson).Cast<TResult>();

        public string ConvertToConcrete(StringReader cson)
        {
            cson.SkipOne();
            string result = cson.TakeUntil(_valueEndChar);
            cson.SkipOne();
            return result;
        }

        public override bool IsCanConvertable(StringReader cson) => cson.StartsWith(_valueStartChar);

        public bool IsCanConvertable(Type type) => type == typeof(string);
    }
}
