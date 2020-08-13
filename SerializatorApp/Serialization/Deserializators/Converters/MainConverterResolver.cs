using SerializatorApp.Serialization.Deserializators.Converters.Numerics;
using SerializatorApp.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public sealed class MainConverterResolver : ConverterBase
    {
        private readonly IConverterCollection _converterCollection;

        public MainConverterResolver() => _converterCollection = new ConverterCollection(new StringConverter(), new ObjectConverter(this), new NullConverter(), new NumericConverterResolver());

        public override TResult Convert<TResult>(CsonReader cson, ITypeNameResolver typeNameResolver) => _converterCollection.Get(cson).Convert<TResult>(cson, typeNameResolver);

        public override bool IsCanConvertable(CsonReader cson) => _converterCollection.Contains(cson);
    }
}
