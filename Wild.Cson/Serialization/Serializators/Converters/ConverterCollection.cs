using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public interface IConverterCollection
    {
        bool Contains(TypeInfo type);

        IConverter Get(TypeInfo type);
    }

    public interface IConcreteValueConverterCollection
    {
        bool Contains(object value);

        IConcreteValueConverter Get(object value);
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

    public class ConcreteValueConverterCollection : IConcreteValueConverterCollection
    {
        private IEnumerable<IConcreteValueConverter> _converters;

        public ConcreteValueConverterCollection(IEnumerable<IConcreteValueConverter> converters) => Init(converters);

        public ConcreteValueConverterCollection(params IConcreteValueConverter[] converters) => Init(converters);

        private void Init(IEnumerable<IConcreteValueConverter> converters)
        {
            _converters = converters;
        }

        public bool Contains(object value) => _converters.Any(c => c.IsConvertable(value));

        public IConcreteValueConverter Get(object value) => _converters.FirstOrDefault(c => c.IsConvertable(value));
    }

    public class ConcreteTypeConverterCollection : IConverterCollection
    {
        private IDictionary<TypeInfo, IConcreteTypeConverter> _converters;

        public ConcreteTypeConverterCollection(IEnumerable<IConcreteTypeConverter> converters) => Init(converters);

        public ConcreteTypeConverterCollection(params IConcreteTypeConverter[] converters) => Init(converters);

        private void Init(IEnumerable<IConcreteTypeConverter> converters)
        {
            _converters = converters.ToDictionary(c => c.ConcreteType);
        }

        public bool Contains(TypeInfo type) => _converters.ContainsKey(type);

        public IConverter Get(TypeInfo type) => GetConcrete(type);
        public IConcreteTypeConverter GetConcrete(TypeInfo type) => _converters.TryGetValue(type, out var converter) ? converter : null;
    }
}
