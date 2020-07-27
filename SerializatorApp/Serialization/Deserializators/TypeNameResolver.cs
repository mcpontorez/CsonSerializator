using SerializatorApp.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators
{
    public sealed class TypeNameResolver : ITypeNameResolver
    {
        public static readonly TypeNameResolver Empty = new TypeNameResolver(new HashSet<string>());

        private HashSet<string> _usings;
        private Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public TypeNameResolver(HashSet<string> usings) => _usings = usings;

        public Type Get(string typeName)
        {
            Type result = null;
            if (_types.TryGetValue(typeName, out result))
                return result;

            result = TypeHelper.Get(typeName);
            if (result == null)
            {
                foreach (var item in _usings)
                {
                    result = TypeHelper.Get($"{item}.{typeName}");
                    if (result != null)
                        break;
                }
            }

            _types.Add(typeName, result);
            return result;
        }
    }
}
