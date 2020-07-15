using SerializatorApp.Serialization.Deserializators.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators
{
    public sealed class MainConverterResolver : ConverterBase
    {
        private readonly IConverterCollection _converterCollection;

        public MainConverterResolver() => _converterCollection = new ConverterCollection(new StringConverter(), new ObjectConverter(this), new NullConverter(), new NumericConverterResolver());

        public override TResult Convert<TResult>(StringReader cson, ITypeNameResolver typeNameResolver) => _converterCollection.Get(cson).Convert<TResult>(cson, typeNameResolver);

        public override bool IsCanConvertable(StringReader cson) => _converterCollection.Contains(cson);
    }
}
