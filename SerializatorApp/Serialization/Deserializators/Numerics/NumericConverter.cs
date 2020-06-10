using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators.Numerics
{
    public class NumericConverterResolver : IConverter
    {
        private readonly IConverterCollection _converterCollection = new ConverterCollection(new ObjectConverter(), new NullConverter(), new NumericConverterResolver());

        public T From<T>(StringReader cson) => _converterCollection.Get(cson).From<T>(cson);

        public bool IsCanConvertable(StringReader cson) => char.IsDigit(cson.GetCurrentChar());
    }
}
