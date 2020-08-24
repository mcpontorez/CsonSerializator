using System;
using System.Reflection;
using Wild.Cson.Serialization.Serializators.Writing;

namespace Wild.Cson.Serialization.Serializators.Converters
{
    public interface IConverterBase
    {
        void Convert(object source, ICsonWriter writer);
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