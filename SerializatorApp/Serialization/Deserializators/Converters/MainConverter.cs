using SerializatorApp.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public class MainConverter
    {
        private readonly UsingConverter _usingConverter = new UsingConverter();
        private readonly IConverter _converterResolver = new MainConverterResolver();

        public T Convert<T>(string cson)
        {
            CsonReader csonReader = new CsonReader(cson);
            csonReader.SkipWhileSeparators();

            HashSet<string> usings = _usingConverter.IsCanConvertable(csonReader) ? _usingConverter.Convert(csonReader) : new HashSet<string>();
            TypeResolver typeResolver = new TypeResolver(usings);

            return _converterResolver.Convert<T>(csonReader, typeResolver);
        }
    }
}
