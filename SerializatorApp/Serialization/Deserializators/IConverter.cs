using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public interface IConverter : IConverterBase
    {
        bool IsCanConvertable(StringReader cson);
    }
}
