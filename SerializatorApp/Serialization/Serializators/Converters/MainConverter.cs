using SerializatorApp.Serialization.Serializators.Writing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class MainConverter
    {
        private readonly IConverter _converter = new MainConverterResolver();

        public string Convert(object source)
        {
            ICsonWriter stringWriter = new CsonWriter();

            _converter.Convert(source, stringWriter);

            string cson = stringWriter.GetString();
            return cson;
        }
    }
}
