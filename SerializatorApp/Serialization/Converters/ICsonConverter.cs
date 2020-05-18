using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Converters
{
    public interface ICsonConverter
    {
        T From<T>(string cson);
        CsonData To(object source);
    }

    public interface ICsonConverter<TCsonItem> : ICsonConverter
    {

    }
}