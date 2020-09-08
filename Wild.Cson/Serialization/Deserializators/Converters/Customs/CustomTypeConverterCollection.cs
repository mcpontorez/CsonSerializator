using System;
using System.Collections.Generic;
using System.Linq;

namespace Wild.Cson.Serialization.Deserializators.Converters.Customs
{
    public interface ICustomTypeConverterCollection
    {
        ICustomTypeConverter Get(Type type);
        bool Contains(Type type);
    }

    public class CustomTypeConverterCollection : ICustomTypeConverterCollection
    {
        private readonly IEnumerable<ICustomTypeConverter> _converters;

        public CustomTypeConverterCollection(IEnumerable<ICustomTypeConverter> converters) => _converters = converters;

        public CustomTypeConverterCollection(params ICustomTypeConverter[] converters) : this((IEnumerable<ICustomTypeConverter>)converters) { }

        public bool Contains(Type type) => _converters.Any(c => c.IsConvertable(type));

        public ICustomTypeConverter Get(Type type) => _converters.FirstOrDefault(c => c.IsConvertable(type));
    }
}