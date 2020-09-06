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
        private static IReadOnlyList<IReadOnlyList<TypeData>> _countNameTypeDataPairs;

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

        public static bool HasManyNamespaces(IEnumerable<string> namespaces, Type type)
        {
            SetTypes();

            string typeName = type.Name, typeNamespace = type.Namespace;
            if (typeNamespace == null)
                return false;

            var typeDatas = _countNameTypeDataPairs[typeName.Length];
            if (typeDatas == null)
                throw new Exception("Unknown type!");

            for (int i = 0; i < typeDatas.Count; i++)
            {
                TypeData typeItem = typeDatas[i];

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

        private static void SetTypes()
        {
            if (_countNameTypeDataPairs == null)
            {
                List<List<TypeData>> countNameTypeDataPairs = new List<List<TypeData>>(30);
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.IsDynamic)
                        continue;
                    Type[] types = assembly.GetExportedTypes();
                    foreach (var typeItem in types)
                    {
                        TypeData typeData = new TypeData(typeItem.Name, typeItem.Namespace, typeItem.FullName);
                        int index = typeData.Name.Length;
                        while (index >= countNameTypeDataPairs.Count)
                        {
                            countNameTypeDataPairs.Add(null);
                        }
                        List<TypeData> currentTypeDatas = countNameTypeDataPairs[index];
                        if (currentTypeDatas == null)
                        {
                            currentTypeDatas = new List<TypeData>();
                            countNameTypeDataPairs[index] = currentTypeDatas;
                        }
                        currentTypeDatas.Add(typeData);
                    }
                }
                _countNameTypeDataPairs = countNameTypeDataPairs;
            }
        }
    }
}
