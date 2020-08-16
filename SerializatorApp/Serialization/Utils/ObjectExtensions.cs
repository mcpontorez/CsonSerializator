using System;

namespace SerializatorApp.Serialization.Utils
{
    public static class ObjectExtensions
    {
        public static TResult WildCast<TResult>(this object target) => (TResult)target;
    }
}
