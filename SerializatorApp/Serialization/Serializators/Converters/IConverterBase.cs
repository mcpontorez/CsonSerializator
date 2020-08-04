using System;
using System.Reflection;
using SerializatorApp.Serialization.Serializators.Writing;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public interface IConverterBase
    {
        void Convert(object source, IStringWriter writer);
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