using Wild.Cson.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wild.Cson.Serialization.Deserializators.Converters.Builtin
{
    public interface IBuiltinTypeConverterCollection
    {
        IBuiltinTypeConverter Get(ICsonReader cson);
        bool Contains(ICsonReader cson);
    }

    public class BuiltinTypeConverterCollection : IBuiltinTypeConverterCollection
    {
        private IEnumerable<IBuiltinTypeConverter> _converters;

        public BuiltinTypeConverterCollection(IEnumerable<IBuiltinTypeConverter> converters) => _converters = converters;

        public BuiltinTypeConverterCollection(params IBuiltinTypeConverter[] converters) : this((IEnumerable<IBuiltinTypeConverter>)converters) { }

        public bool Contains(ICsonReader cson) => _converters.Any(c => c.IsConvertable(cson));

        public IBuiltinTypeConverter Get(ICsonReader cson) => _converters.FirstOrDefault(c => c.IsConvertable(cson));
    }
}