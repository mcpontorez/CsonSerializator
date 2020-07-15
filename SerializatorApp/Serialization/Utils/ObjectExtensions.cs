using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace SerializatorApp.Serialization.Utils
{
    public static class ObjectExtensions
    {
        public static TResult Cast<TResult>(this object target) => (TResult)target;
    }
}
