using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators
{
    public sealed class TypeNameResolver : ITypeNameResolver
    {
        public HashSet<string> _usings;
        private Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public TypeNameResolver(HashSet<string> usings) => _usings = usings;

        public Type Get(string typeName)
        {
            Type result = null;
            if (_types.TryGetValue(typeName, out result))
                return result;

            result = Type.GetType(typeName) ?? GetTypeFromAssemblies(typeName);
            if (result == null)
            {
                var fullTypeNames = _usings.Select(u => u + typeName);
                foreach (var item in fullTypeNames)
                {
                    result = GetTypeFromAssemblies(typeName);
                    if (result != null)
                        break;
                }
            }

            _types.Add(typeName, result);
            return result;
        }

        private Type GetTypeFromAssemblies(string typeName)
        {
            Type result = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                result = assembly.GetType(typeName);
                if (result != null)
                    break;
            }
            return result;
        }
    }
}
