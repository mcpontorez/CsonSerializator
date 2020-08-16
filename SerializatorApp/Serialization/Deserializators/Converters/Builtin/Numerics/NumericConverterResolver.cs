using SerializatorApp.Serialization.Deserializators.Reading;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Builtin.Numerics
{
    public class NumericConverterResolver : IBuiltinTypeConverter
    {
        private readonly IBuiltinTypeConverterCollection _converterCollection = new BuiltinTypeConverterCollection(new Int32Converter(), new SingleConverter());

        public TResult Convert<TResult>(CsonReader cson) => _converterCollection.Get(cson).Convert<TResult>(cson);
        //rewrite to fast IsCanConvertable
        public bool IsCanConvert(CsonReader cson) => _converterCollection.Contains(cson);
    }
}
