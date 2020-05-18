using SerializatorApp.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Converters
{
    public class MainConverter : ICsonConverter
    {
        private readonly ICsonConverter _converter = new CsonItemConverter();
        public T From<T>(string cson)
        {
            throw new NotImplementedException();
        }

        public string To(object source)
        {
            string cson = $"{_converter.To(source)};";

            return cson;
        }
    }
}
