using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public interface IConverterBase
    {
        T From<T>(StringReader cson);
    }
}
