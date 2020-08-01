using System;
using SerializatorApp.Serialization.Serializators.Writing;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class StringConverter : IConcreteConverter
    {
        public void Convert(object source, IStringWriter writer) => writer.Add($"\"{source}\"");
        public bool IsCanConvertable(Type type) => type == ConcreteType;

        public Type ConcreteType { get; } = typeof(string);
    }
}
