using SerializatorApp.Serialization.Serializators.Writing;
using SerializatorApp.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class NullConverter : IConcreteValueConverter
    {
        public void Convert(object source, IStringWriter writer)
        {
            writer.AddNull();
            return;
        }

        public bool IsConvertable(object value) => value == null;
    }
}
