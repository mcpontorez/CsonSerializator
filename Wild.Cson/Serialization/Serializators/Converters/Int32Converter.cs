using System;
using System.Reflection;
using Wild.Cson.Serialization.Serializators.Utils;
using Wild.Cson.Serialization.Serializators.Writing;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public class Int32Converter : IConcreteTypeConverter
    { 
        public Type ConcreteType { get; } = typeof(int);
        public bool IsConvertable(Type type) => type == ConcreteType;
        public bool IsConvertable(object source, Type type) => type == ConcreteType;

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService) => writer.Add(source.ToString());

    }
}
