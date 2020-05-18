using System;

namespace SerializatorApp.Serialization.Models
{
    public class CsonItemBase
    {
        public Type Type { get; private set; }
        public CsonItemBase(Type type)
        {
            Type = type;
        }
    }
}
