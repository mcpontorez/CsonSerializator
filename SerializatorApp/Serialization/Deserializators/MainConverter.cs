using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators
{
    public class MainConverter : IConverterBase
    {
        private readonly UsingConverter _usingConverter = new UsingConverter();
        private readonly IConverter _converterResolver = new MainConverterResolver();

        public T Convert<T>(StringReader cson)
        {
            cson.SkipWhileSeparators();

            HashSet<string> usings = _usingConverter.IsCanConvertable(cson) ? _usingConverter.Convert(cson) : new HashSet<string>();
            TypeNameResolver typeNameResolver = new TypeNameResolver(usings);

            return _converterResolver.Convert<T>(cson, typeNameResolver);
        }

        public bool IsCanConvertable(StringReader cson) => _converterResolver.IsCanConvertable(cson.SkipWhileSeparators());
    }
}
