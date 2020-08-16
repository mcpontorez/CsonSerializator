using System;

namespace SerializatorApp.Serialization.Utils
{
    public static class CollectionExtensions
    {
        public static T[] SetValue<T>(this T[] target, int index, T value)
        {
            target[index] = value;
            return target;
        }
    }
}
