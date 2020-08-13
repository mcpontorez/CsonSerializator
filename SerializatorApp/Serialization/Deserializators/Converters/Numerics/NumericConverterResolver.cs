using SerializatorApp.Serialization.Deserializators.Reading;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Numerics
{
    public class NumericConverterResolver : ConverterBase, IConcreteConverter
    {
        private readonly IConverterCollection _converterCollection = new ConverterCollection(new Int32Converter(), new SingleConverter());

        public override TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver) => _converterCollection.Get(cson).Convert<TResult>(cson, typeResolver);

        public override bool IsCanConvertable(CsonReader cson) => char.IsDigit(cson.CurrentChar);

        public bool IsCanConvertable(Type type) => _converterCollection.Contains(type);
    }
}
