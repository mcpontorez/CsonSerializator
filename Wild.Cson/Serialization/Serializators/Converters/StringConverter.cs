using System;
using System.Reflection;
using Wild.Cson.Serialization.Serializators.Writing;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class StringConverter : IConcreteTypeConverter
    {
        public Type ConcreteType { get; } = typeof(string);
        public bool IsConvertable(Type type) => type == ConcreteType;
        public bool IsConvertable(object source, Type type) => type == ConcreteType;

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService) => writer.Add($"\"{source}\"");
    }
}
