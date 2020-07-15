using System;
using System.ComponentModel;

namespace SerializatorApp.Serialization.Deserializators.Numerics
{
    public class NumericConverterResolver : IConcreteConverter
    {
        private readonly IConverterCollection _converterCollection = new ConverterCollection(new Int32Converter(), new SingleConverter());

        public TResult Convert<TResult>(StringReader cson, ITypeNameResolver typeNameResolver) => _converterCollection.Get(cson).Convert<TResult>(cson, typeNameResolver);

        public bool IsCanConvertable(StringReader cson) => char.IsDigit(cson.CurrentChar);

        public bool IsCanConvertable(Type type) => _converterCollection.Contains(type);
    }
}
