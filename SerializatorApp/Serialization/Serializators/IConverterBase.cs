using System;
using System.IO;
using SerializatorApp.Serialization.Serializators.Writing;
using SerializatorApp.Serializators.Models;

namespace SerializatorApp.Serialization.Serializators
{
    public interface IConverterBase
    {
        void Convert(ConverterData data, IStringWriter writer);
    }

    public interface IConverter : IConverterBase
    {
        bool IsCanConvertable(Type type);
    }
}