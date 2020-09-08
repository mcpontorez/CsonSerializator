using Wild.Cson.Serialization.Deserializators.Converters.Customs;
using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin
{
    public class StringConverter : IBuiltinTypeConverter
    {
        public Type ConcreteType { get; } = typeof(string);

        public TResult Convert<TResult>(CsonReader cson) => ConvertToConcrete(cson).WildCast<TResult>();

        public string ConvertToConcrete(CsonReader cson)
        {
            cson.SkipOne();
            string result = cson.TakeUntil(CharConsts.DoubleQuote);
            cson.SkipOne();
            return result;
        }

        public bool IsConvertable(CsonReader cson) => cson.StartsWith(CharConsts.DoubleQuote);
    }
}
