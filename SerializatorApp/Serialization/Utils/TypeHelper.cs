using System;

namespace SerializatorApp.Serialization.Utils
{
    public static class TypeHelper
    {
        public static Type Get(string name)
        {
            Type result = Type.GetType(name);
            if (result == null)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    result = assembly.GetType(name);
                    if (result != null)
                        break;
                }
            }
            return result;
        }

        public static bool Exists(string name) => Get(name) != null;
    }
}
