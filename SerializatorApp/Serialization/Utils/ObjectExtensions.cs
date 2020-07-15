using System;

namespace SerializatorApp.Serialization.Utils
{
    public static class ObjectExtensions
    {
        public static TResult Cast<TResult>(this object target) => (TResult)target;
    }
}
