using System;
using SerializatorApp.Serializators.Models;

namespace SerializatorApp.Serialization.Serializators
{
    public interface IConverterBase
    {
        CsonData To(CsData csData);
    }

    public interface IConverter : IConverterBase
    {
        bool IsCanConvertable(Type type);
    }
}