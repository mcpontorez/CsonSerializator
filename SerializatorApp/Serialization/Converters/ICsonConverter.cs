using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Converters
{
    public interface ICsonConverterBase
    {
        T From<T>(string cson);
        CsonData To(object source);
    }

    public interface ICsonConverter : ICsonConverterBase
    {
        CsonData To(CsData csData);
    }
}