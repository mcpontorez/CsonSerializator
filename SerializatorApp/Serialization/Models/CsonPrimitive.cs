using System;

namespace SerializatorApp.Serialization.Models
{
    public class CsonPrimitive : CsonItemBase
    {
        public readonly string Value;
        public CsonPrimitive(Type type, string value) : base(type)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
