using SerializatorApp.Serialization.Deserializators.Converters.Customs;
using SerializatorApp.Serialization.Deserializators.Converters.Numerics;
using SerializatorApp.Serialization.Deserializators.Reading;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public sealed class MainConverterResolver : ConverterBase
    {
        private readonly IConverterCollection _converterCollection;

        public MainConverterResolver() => _converterCollection = new ConverterCollection(new StringConverter(), new ObjectConverter(this), new NullConverter(), new NumericConverterResolver());

        public override TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver) => _converterCollection.Get(cson).Convert<TResult>(cson, typeResolver);

        public override bool IsCanConvertable(CsonReader cson) => _converterCollection.Contains(cson);
    }
}
