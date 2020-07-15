using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators
{
    public class MainConverter : ConverterBase
    {
        private readonly UsingConverter _usingConverter = new UsingConverter();
        private readonly IConverterBase _converterResolver = new MainConverterResolver();

        public override T From<T>(StringReader cson)
        {
            cson.SkipWhileSeparators();

            HashSet<string> usings = _usingConverter.IsCanConvertable(cson) ? _usingConverter.From(cson) : new HashSet<string>();
            TypeNameResolver typeNameResolver = new TypeNameResolver(usings);

            return _converterResolver.Convert<T>(cson);
        }

        public override bool IsCanConvertable(StringReader cson) => _converterResolver.IsCanConvertable(cson.SkipWhileSeparators());
    }
}
