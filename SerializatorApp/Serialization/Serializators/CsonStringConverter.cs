using SerializatorApp.Serializators.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators
{
    public class CsonStringConverter : ICsonConverter
    {
        public T From<T>(string cson)
        {
            throw new NotImplementedException();
        }

        public CsonData To(object source) => new CsonData(typeof(string), $"\"{source}\"");

        public CsonData To(CsData csData) => To(csData.Source);
    }
}
