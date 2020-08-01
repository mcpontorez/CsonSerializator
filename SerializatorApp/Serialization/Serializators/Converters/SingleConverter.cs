using SerializatorApp.Serialization.Serializators.Writing;
using System;
using System.Globalization;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class SingleConverter : IConcreteConverter
    {
        public Type ConcreteType { get; } = typeof(float);
        public bool IsCanConvertable(Type type) => type == ConcreteType;

        public void Convert(object source, IStringWriter writer) => writer.Add($"{((float)source).ToString(CultureInfo.InvariantCulture)}F");
    }
}
