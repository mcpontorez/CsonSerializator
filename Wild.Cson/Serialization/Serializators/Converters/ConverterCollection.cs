using System;
using System.Collections.Generic;
using System.Linq;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public interface IConverterCollection
    {
        bool Contains(object source, Type type);

        IConverter Get(object source, Type type);
    }

    public interface IConcreteValueConverterCollection
    {
        bool Contains(object source);

        IConcreteValueConverter Get(object source);
    }

    public class ConverterCollection : IConverterCollection
    {
        private IConverter[] _converters;

        public ConverterCollection(params IConverter[] converters) => _converters = converters;

        public bool Contains(object source, Type type) => _converters.Any(c => c.IsConvertable(source, type));

        public IConverter Get(object source, Type type)
        {
            for (int i = 0; i < _converters.Length; i++)
            {
                var converter = _converters[i];
                if (converter.IsConvertable(source, type))
                    return converter;
            }
            return null;
        }
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

        public bool Contains(object source) => _converters.Any(c => c.IsConvertable(source));

        public IConcreteValueConverter Get(object source) => _converters.FirstOrDefault(c => c.IsConvertable(source));
    }

    public class ConcreteTypeConverterCollection : IConverterCollection
    {
        private IDictionary<Type, IConcreteTypeConverter> _converters;

        public ConcreteTypeConverterCollection(IEnumerable<IConcreteTypeConverter> converters) => Init(converters);

        public ConcreteTypeConverterCollection(params IConcreteTypeConverter[] converters) => Init(converters);

        private void Init(IEnumerable<IConcreteTypeConverter> converters)
        {
            _converters = converters.ToDictionary(c => c.ConcreteType);
        }

        public bool Contains(object source, Type type) => _converters.ContainsKey(type);

        public IConcreteTypeConverter GetConcrete(Type type) => _converters.TryGetValue(type, out var converter) ? converter : null;

        public IConverter Get(object source, Type type) => (IConverter)GetConcrete(type);
    }
}
