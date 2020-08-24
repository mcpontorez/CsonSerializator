using Wild.Cson.Serialization.Serializators.Writing;
using System;
using System.Globalization;
using System.Reflection;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class SingleConverter : IConcreteTypeConverter
    {
        public TypeInfo ConcreteType { get; } = typeof(float).GetTypeInfo();
        public bool IsConvertable(TypeInfo type) => type == ConcreteType;

        public void Convert(object source, ICsonWriter writer) => writer.Add($"{((float)source).ToString(CultureInfo.InvariantCulture)}F");
    }
}
