using Wild.Cson.Serialization.Serializators.Writing;
using System;
using System.Globalization;
using System.Reflection;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class SingleConverter : IConcreteTypeConverter
    {
        public Type ConcreteType { get; } = typeof(float);
        public bool IsConvertable(Type type) => type == ConcreteType;
        public bool IsConvertable(object source, Type type) => type == ConcreteType;

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService) => writer.Add($"{((float)source).ToString(CultureInfo.InvariantCulture)}F");
    }
}
