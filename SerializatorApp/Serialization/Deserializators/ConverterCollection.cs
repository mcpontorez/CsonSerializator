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
        private IEnumerable<IConcreteConverter> _concreteConverters;
        public ConverterCollection(IEnumerable<IConverter> converters) => Init(converters);

        public ConverterCollection(params IConverter[] converters) => Init(converters);

        private void Init(IEnumerable<IConverter> converters)
        {
            _converters = converters;
            _concreteConverters = _converters.Where(c => c is IConcreteConverter).Cast<IConcreteConverter>().ToArray();
        }

        public bool Contains(StringReader cson) => _converters.Any(c => c.IsCanConvertable(cson));

        public bool Contains(Type type) => _concreteConverters.Any(c => c.IsCanConvertable(type));

        public IConverter Get(StringReader cson) => _converters.FirstOrDefault(c => c.IsCanConvertable(cson));

        public IConverter Get(Type type) => _concreteConverters.FirstOrDefault(c => c.IsCanConvertable(type));
    }
}
