using Wild.Cson.Serialization.Deserializators.Reading;
using System;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin.Numerics
{
    public class NumericConverterResolver : IBuiltinTypeConverter
    {
        private readonly IBuiltinTypeConverterCollection _converterCollection = new BuiltinTypeConverterCollection(new Int32Converter(), new SingleConverter());

        public TResult Convert<TResult>(ICsonReader cson) => _converterCollection.Get(cson).Convert<TResult>(cson);

        public bool IsConvertable(ICsonReader cson)
        {
            char currentChar = cson.CurrentChar;
            return currentChar == CharConsts.Minus || char.IsDigit(currentChar);
            //return _converterCollection.Contains(cson);
        }
    }
}
