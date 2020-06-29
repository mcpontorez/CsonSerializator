using SerializatorApp.Serializators.Models;
using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Serializators
{
    public interface ICsonConverterBase
    {
        CsonData To(object source);
    }

    public interface ICsonConverter : ICsonConverterBase
    {
        CsonData To(CsData csData);
    }
}