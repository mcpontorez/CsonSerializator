﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public interface IConverterCollection
    {
        bool Contains(Type type);

        IConverter Get(Type type);
    }

    public interface IConcreteValueConverterCollection
    {
        bool Contains(object value);

        IConcreteValueConverter Get(object value);
    }

    public class ConverterCollection : IConverterCollection
    {
        private IConverter[] _converters;

        public ConverterCollection(params IConverter[] converters) => _converters = converters;

        public bool Contains(Type type) => _converters.Any(c => c.IsConvertable(type));

        public IConverter Get(Type type)
        {
            for (int i = 0; i < _converters.Length; i++)
            {
                var converter = _converters[i];
                if (converter.IsConvertable(type))
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

        public bool Contains(object value) => _converters.Any(c => c.IsConvertable(value));

        public IConcreteValueConverter Get(object value) => _converters.FirstOrDefault(c => c.IsConvertable(value));
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

        public bool Contains(Type type) => _converters.ContainsKey(type);

        public IConverter Get(Type type) => GetConcrete(type);
        public IConcreteTypeConverter GetConcrete(Type type) => _converters.TryGetValue(type, out var converter) ? converter : null;
    }
}
