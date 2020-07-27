using SerializatorApp.Serializators.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Serializators
{
    public sealed class ConverterResolver : IConverter
    {
        private readonly IConverterCollection _converterCollection;

        public ConverterResolver() => _converterCollection = new ConverterCollection(new StringConverter(), new ObjectConverter(this), new NullConverter(), new NumericConverterResolver());

        public CsonData Convert<TResult>(StringReader cson) => _converterCollection.Get(cson).Convert<TResult>(cson, typeNameResolver);

        public bool IsCanConvertable(Type type) => _converterCollection.Contains(type);
    }
}
