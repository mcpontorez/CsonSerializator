using System;
using System.Collections.Generic;
using System.Linq;

namespace SerializatorApp.Serialization.Serializators
{
    public interface IConverterCollection
    {
        bool Contains(Type type);

        IConverter Get(Type type);
    }

    public class ConverterCollection : IConverterCollection
    {
        private IEnumerable<IConverter> _converters;
        
        public ConverterCollection(IEnumerable<IConverter> converters) => Init(converters);

        public ConverterCollection(params IConverter[] converters) => Init(converters);

        private void Init(IEnumerable<IConverter> converters)
        {
            _converters = converters;
        }

        public bool Contains(Type type) => _converters.Any(c => c.IsCanConvertable(type));

        public IConverter Get(Type type) => _converters.FirstOrDefault(c => c.IsCanConvertable(type));
    }
}
