using SerializatorApp.Serialization.Serializators.Writing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class MainConverter
    {
        private readonly IConverter _converter = new ObjectConverter();

        public string Convert(object source)
        {
            IStringWriter stringWriter = new StringWriter();

            _converter.Convert(source, stringWriter);

            string cson = stringWriter.GetString();
            return cson;
        }
    }
}
