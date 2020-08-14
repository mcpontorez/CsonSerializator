﻿using SerializatorApp.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public class ConverterCollection : IConverterCollection
    {
        private IEnumerable<IConverter> _converters;
        private IEnumerable<IConcreteTypeConverter> _concreteConverters;
        public ConverterCollection(IEnumerable<IConverter> converters) => Init(converters);

        public ConverterCollection(params IConverter[] converters) => Init(converters);

        private void Init(IEnumerable<IConverter> converters)
        {
            _converters = converters;
            _concreteConverters = _converters.Where(c => c is IConcreteTypeConverter).Cast<IConcreteTypeConverter>().ToArray();
        }

        public bool Contains(CsonReader cson) => _converters.Any(c => c.IsCanConvertable(cson));

        public bool Contains(Type type) => _concreteConverters.Any(c => c.IsCanConvertable(type));

        public IConverter Get(CsonReader cson) => _converters.FirstOrDefault(c => c.IsCanConvertable(cson));

        public IConverter Get(Type type) => _concreteConverters.FirstOrDefault(c => c.IsCanConvertable(type));
    }
}
