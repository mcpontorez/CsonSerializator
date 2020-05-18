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

        public string To(object source) =>  $"{source.ToString()}F";
    }
}
