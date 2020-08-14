using SerializatorApp.Serialization.Deserializators.Converters.Customs;
using SerializatorApp.Serialization.Deserializators.Reading;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public interface IConverterBase
    {
        TResult Convert<TResult>(CsonReader cson);
    }

    public interface IConverter : IConverterBase
    {
        TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver);
        bool IsCanConvertable(CsonReader cson);
    }

    public interface ISomeTypeConverter : IConverter
    {
        TResult Convert<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver);
        bool IsCanConvertable(Type type);
    }

    public interface IConcreteTypeConverter : IConverter
    {
        Type ConcreteType { get; }
    }

    public interface IConcreteTypeConverter<TResult> : IConcreteTypeConverter
    {
        TResult ConvertToConcrete(CsonReader cson);
    }
}
