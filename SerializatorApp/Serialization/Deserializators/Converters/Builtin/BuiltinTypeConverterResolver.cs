using SerializatorApp.Serialization.Deserializators.Converters.Builtin.Numerics;
using SerializatorApp.Serialization.Deserializators.Converters.Customs;
using SerializatorApp.Serialization.Deserializators.Reading;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Builtin
{
    public sealed class BuiltinTypeConverterResolver : IConverterResolver
    {
        private IBuiltinTypeConverterCollection _converters = new BuiltinTypeConverterCollection(new NullConverter(), new StringConverter(), new NumericConverterResolver());

        public bool IsCanConvertable(CsonReader cson) => _converters.Contains(cson);

        public TResult Convert<TResult>(CsonReader cson) => _converters.Get(cson).Convert<TResult>(cson);

        public TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver) => Convert<TResult>(cson);
    }
}