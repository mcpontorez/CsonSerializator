using System;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public interface ITypeNameResolver
    {
        Type Convert(string typeName);
    }
}
