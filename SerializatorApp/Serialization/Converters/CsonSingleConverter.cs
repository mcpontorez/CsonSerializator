using SerializatorApp.Serialization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Converters
{
    public class CsonSingleConverter : ICsonConverter
    {
        public T From<T>(string cson)
        {
            throw new NotImplementedException();
        }

        public CsonData To(object source) => new CsonData(new HashSet<Type> { typeof(float) }, $"{source.ToString()}F");
    }
}
