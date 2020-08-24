using System;
using System.Reflection;
using Wild.Cson.Serialization.Serializators.Writing;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class StringConverter : IConcreteTypeConverter
    {
        public bool IsConvertable(TypeInfo type) => type == ConcreteType;
        public TypeInfo ConcreteType { get; } = typeof(string).GetTypeInfo();

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService) => writer.Add($"\"{source}\"");
    }
}
