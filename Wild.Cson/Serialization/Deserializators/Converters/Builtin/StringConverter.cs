using Wild.Cson.Serialization.Deserializators.Converters.Customs;
using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin
{
    public class StringConverter : IBuiltinTypeConverter
    {
        public Type ConcreteType { get; } = typeof(string);

        public TResult Convert<TResult>(ICsonReader cson) => ConvertToConcrete(cson).WildCast<TResult>();

        public string ConvertToConcrete(ICsonReader cson)
        {
            cson.Skip(CharConsts.DoubleQuote);
            string result = cson.TakeUntil(CharConsts.DoubleQuote);
            cson.Skip(CharConsts.DoubleQuote);
            return result;
        }

        public bool IsConvertable(ICsonReader cson) => cson.StartsWith(CharConsts.DoubleQuote);
    }
}
