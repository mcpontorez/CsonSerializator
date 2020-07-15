using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public interface ITypeNameResolver
    {
        Type Get(string typeName);
    }
}
