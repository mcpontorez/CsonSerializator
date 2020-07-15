using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public interface IConverterBase
    {
        TResult Convert<TResult>(StringReader cson);

        bool IsCanConvertable(StringReader cson);
    }

    public interface IConverter : IConverterBase
    {
        TResult Convert<TResult>(StringReader cson, ITypeNameResolver typeNameResolver);
    }

    public interface IConcreteConverter : IConverter
    {
        bool IsCanConvertable(Type type);
    }

    public interface IConcreteConverter<TResult> : IConcreteConverter
    {
        TResult ConvertToConcrete(StringReader cson);
    }
}
