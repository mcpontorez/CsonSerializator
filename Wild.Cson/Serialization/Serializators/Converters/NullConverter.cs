using Wild.Cson.Serialization.Serializators.Writing;
using Wild.Cson.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class NullConverter : IConcreteValueConverter
    {
        public void Convert(object source, ICsonWriter writer)
        {
            writer.AddNull();
            return;
        }

        public bool IsConvertable(object value) => value == null;
    }
}
