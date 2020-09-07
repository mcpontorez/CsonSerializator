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
        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService)
        {
            writer.AddNull();
            return;
        }

        public bool IsConvertable(object source) => source == null;

        public bool IsConvertable(object source, Type type) => IsConvertable(source);
    }
}
