using SerializatorApp.Serialization.Deserializators.Converters.Customs;
using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public class MainConverter
    {
        private readonly UsingConverter _usingConverter = new UsingConverter();
        private readonly IConverterResolver _converterResolver = new MainConverterResolver();

        public TResult Convert<TResult>(string cson)
        {
            CsonReader csonReader = new CsonReader(cson);
            csonReader.SkipWhileSeparators();

            ITypeResolver typeResolver;
            if (_usingConverter.IsCanConvertable(csonReader))
                typeResolver = new TypeResolver(_usingConverter.Convert(csonReader));
            else typeResolver = TypeResolver.InstanceWhithoutUsings;

            TResult result = _converterResolver.Convert<TResult>(csonReader, typeResolver);

            csonReader.SkipWhileSeparators().Skip(CharConsts.Semicolon);

            return result;
        }
    }
}
