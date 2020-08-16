using System;

namespace SerializatorApp.Serialization.Utils
{
    public static class ObjectExtensions
    {
        public static TResult SuperCast<TResult>(this object target) => (TResult)target;
    }
}
