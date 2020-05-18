using System;

namespace SerializatorApp.Serialization.Converters
{
    public interface ICsonConverter
    {
        T From<T>(string cson);
        string To(object source);
    }

    public interface ICsonConverter<TCsonItem> : ICsonConverter
    {

    }
}