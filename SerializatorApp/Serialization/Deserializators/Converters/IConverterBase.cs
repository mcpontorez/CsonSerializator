using SerializatorApp.Serialization.Deserializators.Converters.Customs;
using SerializatorApp.Serialization.Deserializators.Reading;
using System;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public interface IConverterBase
    {
        TResult Convert<TResult>(CsonReader cson);

        bool IsCanConvertable(CsonReader cson);
    }

    public interface IConverter : IConverterBase
    {
        TResult Convert<TResult>(CsonReader cson, ITypeResolver typeResolver);
    }

    public interface IConcreteConverter : IConverter
    {
        bool IsCanConvertable(Type type);
    }

    public interface IConcreteConverter<TResult> : IConcreteConverter
    {
        TResult ConvertToConcrete(CsonReader cson);
    }
}
