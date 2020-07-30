using System;
using System.Collections.Generic;

namespace SerializatorApp.Serializators.Models
{
    public struct ConverterData
    {
        public readonly object Source;
        public readonly uint NestedLevel;
        public ConverterData(object source, uint nestedLevel)
        {
            Source = source;
            NestedLevel = nestedLevel;
        }
    }
}
