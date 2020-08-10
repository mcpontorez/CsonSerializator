using System;

namespace SerializatorApp.Serialization.Deserializators
{
    public interface ITypeNameResolver
    {
        Type Convert(string typeName);
    }
}
