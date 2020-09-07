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

    public interface IConverter : IConverterBase
    {
        bool IsConvertable(object source, Type type);
    }

    public interface IConcreteValueConverter : IConverter
    {
        bool IsConvertable(object source);
    }

    public interface IConcreteTypeConverter : IConverter
    {
        Type ConcreteType { get; }
    }
}