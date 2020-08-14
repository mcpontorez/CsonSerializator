using SerializatorApp.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SerializatorApp.Serialization.Deserializators.Converters.Builtin
{
    public interface IBuiltinTypeConverterCollection
    {
        IBuiltinTypeConverter Get(CsonReader cson);
        bool Contains(CsonReader cson);
    }

    public class BuiltinTypeConverterCollection : IBuiltinTypeConverterCollection
    {
        private IEnumerable<IBuiltinTypeConverter> _converters;

        public BuiltinTypeConverterCollection(IEnumerable<IBuiltinTypeConverter> converters) => _converters = converters;

        public BuiltinTypeConverterCollection(params IBuiltinTypeConverter[] converters) : this((IEnumerable<IBuiltinTypeConverter>)converters) { }

        public bool Contains(CsonReader cson) => _converters.Any(c => c.IsCanConvertable(cson));

        public IBuiltinTypeConverter Get(CsonReader cson) => _converters.FirstOrDefault(c => c.IsCanConvertable(cson));
    }
}