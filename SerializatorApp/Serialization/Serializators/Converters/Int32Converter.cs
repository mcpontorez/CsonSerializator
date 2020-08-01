using System;
using SerializatorApp.Serialization.Serializators.Writing;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class Int32Converter : IConcreteConverter
    {
        public Type ConcreteType { get; } = typeof(int);

        public void Convert(object source, IStringWriter writer) => writer.Add(source.ToString());

        public bool IsCanConvertable(Type type) => type == ConcreteType;
    }
}
