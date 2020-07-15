using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public interface IConverterBase
    {
        TResult Convert<TResult>(StringReader cson, ITypeNameResolver typeNameResolver));

        bool IsCanConvertable(StringReader cson);
    }

    public interface IConcreteConverter : IConverterBase
    {
        bool IsCanConvertable(Type type);
    }

    public interface IConcreteConverter<TResult> : IConcreteConverter
    {
        TResult ConvertToConcrete(StringReader cson);
    }
}
