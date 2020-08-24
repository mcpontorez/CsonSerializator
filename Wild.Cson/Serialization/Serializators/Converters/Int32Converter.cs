using System;
using System.Reflection;
using Wild.Cson.Serialization.Serializators.Writing;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class Int32Converter : IConcreteTypeConverter
    {
        public TypeInfo ConcreteType { get; } = typeof(int).GetTypeInfo();

        public void Convert(object source, ICsonWriter writer) => writer.Add(source.ToString());

        public bool IsConvertable(TypeInfo type) => type == ConcreteType;
    }
}
