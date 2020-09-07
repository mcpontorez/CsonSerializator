using System;
using System.Reflection;
using Wild.Cson.Serialization.Serializators.Writing;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class Int32Converter : IConcreteTypeConverter
    {
        public bool IsConvertable(Type type) => type == ConcreteType;
        public Type ConcreteType { get; } = typeof(int);

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService) => writer.Add(source.ToString());
    }
}
