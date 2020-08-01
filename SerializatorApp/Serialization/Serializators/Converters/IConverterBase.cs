using System;
using SerializatorApp.Serialization.Serializators.Writing;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public interface IConverterBase
    {
        void Convert(object source, IStringWriter writer);
    }

    public interface IConverter : IConverterBase
    {
        bool IsCanConvertable(Type type);
    }

    public interface IConcreteConverter : IConverter
    {
        Type ConcreteType { get; }
    }
}