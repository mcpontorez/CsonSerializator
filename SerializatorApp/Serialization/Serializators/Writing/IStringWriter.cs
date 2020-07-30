using System;
using System.Collections.Generic;
using System.Text;

namespace SerializatorApp.Serialization.Serializators.Writing
{
    public interface IStringWriter
    {
        IStringWriter Add(string value);

        IStringWriter Add(object value);

        IStringWriter Add(Type type);

        IStringWriter AddLine();

        IStringWriter AddTabLevel(int value);

        string GetString();
    }
}