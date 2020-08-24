using System;
using System.Reflection;
using Wild.Cson.Serialization.Serializators.Writing;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class StringConverter : IConcreteTypeConverter
    {
        public void Convert(object source, ICsonWriter writer) => writer.Add($"\"{source}\"");
        public bool IsConvertable(TypeInfo type) => type == ConcreteType;

        public TypeInfo ConcreteType { get; } = typeof(string).GetTypeInfo();
    }
}
