using SerializatorApp.Serialization.Deserializators.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators
{
    public sealed class MainConverterResolver : IConverterBase
    {
        private readonly IConverterCollection _converterCollection;

        public MainConverterResolver() => _converterCollection = new ConverterCollection(new StringConverter(), new ObjectConverter(this), new NullConverter(), new NumericConverterResolver());

        public T Convert<T>(StringReader cson) => _converterCollection.Get(cson).Convert<T>(cson);

        public bool IsCanConvertable(StringReader cson) => _converterCollection.Contains(cson);
    }
}
