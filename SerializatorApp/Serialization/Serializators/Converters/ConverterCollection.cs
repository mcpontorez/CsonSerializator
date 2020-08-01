using System;
using System.Collections.Generic;
using System.Linq;

namespace SerializatorApp.Serialization.Serializators.Converters
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

    public class ConcreteConverterCollection : IConverterCollection
    {
        private IDictionary<Type, IConcreteConverter> _converters;

        public ConcreteConverterCollection(IEnumerable<IConcreteConverter> converters) => Init(converters);

        public ConcreteConverterCollection(params IConcreteConverter[] converters) => Init(converters);

        private void Init(IEnumerable<IConcreteConverter> converters)
        {
            _converters = converters.ToDictionary(c => c.ConcreteType);
        }

        public bool Contains(Type type) => _converters.ContainsKey(type);

        public IConverter Get(Type type) => GetConcrete(type);
        public IConcreteConverter GetConcrete(Type type) => _converters.TryGetValue(type, out var converter) ? converter : null;
    }
}
