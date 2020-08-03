using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public interface IConverterCollection
    {
        bool Contains(TypeInfo type);

        IConverter Get(TypeInfo type);
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

        public bool Contains(TypeInfo type) => _converters.Any(c => c.IsConvertable(type));

        public IConverter Get(TypeInfo type) => _converters.FirstOrDefault(c => c.IsConvertable(type));
    }

    public class ConcreteConverterCollection : IConverterCollection
    {
        private IDictionary<TypeInfo, IConcreteConverter> _converters;

        public ConcreteConverterCollection(IEnumerable<IConcreteConverter> converters) => Init(converters);

        public ConcreteConverterCollection(params IConcreteConverter[] converters) => Init(converters);

        private void Init(IEnumerable<IConcreteConverter> converters)
        {
            _converters = converters.ToDictionary(c => c.ConcreteType);
        }

        public bool Contains(TypeInfo type) => _converters.ContainsKey(type);

        public IConverter Get(TypeInfo type) => GetConcrete(type);
        public IConcreteConverter GetConcrete(TypeInfo type) => _converters.TryGetValue(type, out var converter) ? converter : null;
    }
}
