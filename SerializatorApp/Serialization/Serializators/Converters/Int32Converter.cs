using System;
using System.Reflection;
using SerializatorApp.Serialization.Serializators.Writing;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class Int32Converter : IConcreteTypeConverter
    {
        public TypeInfo ConcreteType { get; } = typeof(int).GetTypeInfo();

        public void Convert(object source, IStringWriter writer) => writer.Add(source.ToString());

        public bool IsConvertable(TypeInfo type) => type == ConcreteType;
    }
}
