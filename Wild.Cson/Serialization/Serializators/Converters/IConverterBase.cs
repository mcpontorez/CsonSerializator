using System;
using System.Reflection;
using Wild.Cson.Serialization.Serializators.Writing;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public interface IConverterBase
    {
        void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService);
    }

    public interface IConcreteValueConverter : IConverterBase
    {
        bool IsConvertable(object value);
    }

    public interface IConverter : IConverterBase
    {
        bool IsConvertable(TypeInfo type);
    }

    public interface IConcreteTypeConverter : IConverter
    {
        TypeInfo ConcreteType { get; }
    }
}