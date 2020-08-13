using System;

namespace SerializatorApp.Serialization.Deserializators.Converters.Customs
{
    public interface ITypeResolver
    {
        Type Convert(string typeName);
    }
}
