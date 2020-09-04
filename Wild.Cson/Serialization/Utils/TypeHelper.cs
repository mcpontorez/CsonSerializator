using System;
using System.Collections.Generic;
using System.Reflection;

namespace Wild.Cson.Serialization.Utils
{
    public class TypeData
    {
        public readonly string Name, Namespace, FullName;
        public TypeData(string name, string @namespace, string fullName)
        {
            Name = name;
            Namespace = @namespace;
            FullName = fullName;
        }
    }

    public static class TypeHelper
    {
        private static IReadOnlyList<TypeData> _allTypes;

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

        public static bool Exists(IEnumerable<string> namespaces, Type type)
        {
            if(_allTypes == null)
            {
                List<TypeData> allTypes = new List<TypeData>();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.IsDynamic)
                        continue;
                    Type[] types = assembly.GetExportedTypes();
                    foreach (var typeItem in types)
                    {
                        allTypes.Add(new TypeData(typeItem.Name, typeItem.Namespace, typeItem.FullName));
                    }
                }
                _allTypes = allTypes;
            }

            if (type.Namespace == null)
                return false;

            string typeName = type.Name, typeNamespace = type.Namespace;
            for (int i = 0; i < _allTypes.Count; i++)
            {
                TypeData typeItem = _allTypes[i];

                if (typeItem.Name == typeName && typeItem.Namespace != typeNamespace)
                {
                    if (typeItem.Namespace == null)
                        return true;
                    foreach (var @namespace in namespaces)
                    {
                        if (typeItem.Namespace == @namespace)
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
