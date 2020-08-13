using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public class StringConverter : ConverterBase, IConcreteConverter<string>
    {
        private const char _valueStartChar = '"', _valueEndChar = '"';

        public override TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver) => ConvertToConcrete(cson).Cast<TResult>();

        public string ConvertToConcrete(CsonReader cson)
        {
            cson.SkipOne();
            string result = cson.TakeUntil(_valueEndChar);
            cson.SkipOne();
            return result;
        }

        public override bool IsCanConvertable(CsonReader cson) => cson.StartsWith(_valueStartChar);

        public bool IsCanConvertable(Type type) => type == typeof(string);
    }
}
