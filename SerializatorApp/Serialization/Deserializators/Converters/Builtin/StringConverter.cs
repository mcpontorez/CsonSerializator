using SerializatorApp.Serialization.Deserializators.Converters.Customs;
using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Builtin
{
    public class StringConverter : IBuiltinTypeConverter
    {
        public Type ConcreteType { get; } = typeof(string);

        public TResult Convert<TResult>(CsonReader cson) => ConvertToConcrete(cson).Cast<TResult>();

        public string ConvertToConcrete(CsonReader cson)
        {
            cson.SkipOne();
            string result = cson.TakeUntil(CharConsts.DoubleQuote);
            cson.SkipOne();
            return result;
        }

        public bool IsCanConvertable(CsonReader cson) => cson.StartsWith(CharConsts.DoubleQuote);
    }
}
