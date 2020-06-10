using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators
{
    public class ConverterCollection : IConverterCollection
    {
        private IEnumerable<IConverter> _converters;
        public ConverterCollection(IEnumerable<IConverter> converters)
        {
            _converters = converters;
        }

        public ConverterCollection(params IConverter[] converters)
        {
            _converters = converters;
        }

        public bool Contains(StringReader cson) => _converters.Any(c => c.IsCanConvertable(cson));

        public IConverter Get(StringReader cson) => _converters.FirstOrDefault(c => c.IsCanConvertable(cson));
    }
}
