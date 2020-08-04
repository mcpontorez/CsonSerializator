using SerializatorApp.Serialization.Serializators.Writing;
using System;
using System.Globalization;
using System.Reflection;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public class SingleConverter : IConcreteTypeConverter
    {
        public TypeInfo ConcreteType { get; } = typeof(float).GetTypeInfo();
        public bool IsConvertable(TypeInfo type) => type == ConcreteType;

        public void Convert(object source, IStringWriter writer) => writer.Add($"{((float)source).ToString(CultureInfo.InvariantCulture)}F");
    }
}
