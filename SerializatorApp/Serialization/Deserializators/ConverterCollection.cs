using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators
{
    public class ConverterCollection : IConverterCollection
    {
        private IEnumerable<IConverterBase> _converters;
        private IEnumerable<IConcreteConverter> _concreteConverters;
        public ConverterCollection(IEnumerable<IConverterBase> converters)
        {
            _converters = converters;
            _concreteConverters = _converters.Where(c => c is IConcreteConverter).Cast<IConcreteConverter>();
        }

        public ConverterCollection(params IConverterBase[] converters)
        {
            _converters = converters;
        }

        public bool Contains(StringReader cson) => _converters.Any(c => c.IsCanConvertable(cson));

        public bool Contains(Type type) => _concreteConverters.Any(c => c.IsCanConvertable(type));

        public IConverterBase Get(StringReader cson) => _converters.FirstOrDefault(c => c.IsCanConvertable(cson));
    }
}
