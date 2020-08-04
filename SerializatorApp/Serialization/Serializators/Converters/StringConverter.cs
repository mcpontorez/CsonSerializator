using System;
using System.Reflection;
using SerializatorApp.Serialization.Serializators.Writing;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class StringConverter : IConcreteTypeConverter
    {
        public void Convert(object source, IStringWriter writer) => writer.Add($"\"{source}\"");
        public bool IsConvertable(TypeInfo type) => type == ConcreteType;

        public TypeInfo ConcreteType { get; } = typeof(string).GetTypeInfo();
    }
}
