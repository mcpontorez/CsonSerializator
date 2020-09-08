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
        private static List<Dictionary<string, List<TypeData>>> _countNameTypeDataPairs;

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

            string typeNamespace = type.Namespace;
            if (ReferenceEquals(typeNamespace, null))
                return false;
            string typeName = type.Name;

            var nameTypeDatas = _countNameTypeDataPairs[typeName.Length];
            if (ReferenceEquals(nameTypeDatas, null) || !nameTypeDatas.TryGetValue(typeName, out List<TypeData> typeDatas))
                throw new Exception("Unknown type!");

            for (int i = 0; i < typeDatas.Count; i++)
            {
                TypeData typeItem = typeDatas[i];

                if (!ReferenceEquals(typeItem.Namespace, typeNamespace))
                {
                    if (ReferenceEquals(typeItem.Namespace, null))
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
                var countNameTypeDataPairs = new List<Dictionary<string, List<TypeData>>>(30);
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
                        Dictionary<string, List<TypeData>> currentNameTypeDatas = countNameTypeDataPairs[index];
                        if (currentNameTypeDatas == null)
                        {
                            currentNameTypeDatas = new Dictionary<string, List<TypeData>>();
                            countNameTypeDataPairs[index] = currentNameTypeDatas;
                        }
                        if(currentNameTypeDatas.TryGetValue(typeData.Name, out List<TypeData> currentTypeDatas))
                        { }
                        else
                        {
                            currentTypeDatas = new List<TypeData>();
                            currentNameTypeDatas.Add(typeData.Name, currentTypeDatas);
                        }
                        currentTypeDatas.Add(typeData);
                    }
                }
                _countNameTypeDataPairs = countNameTypeDataPairs;
            }
        }
    }
}
