using Wild.Cson.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wild.Cson.Serialization.Serializators.Writing
{
    public struct TypesData
    {
        public readonly IReadOnlyCollection<string> Namespaces;
        public readonly IReadOnlyDictionary<Type, bool> IsWritesFullNames;

        public TypesData(IReadOnlyCollection<string> namespaces, IReadOnlyDictionary<Type, bool> isWritesFullNames)
        {
            Namespaces = namespaces;
            IsWritesFullNames = isWritesFullNames;
        }
    }

    public interface ITypeService
    {
        void Add(Type type);

        TypesData GetTypesData();
    }
    public sealed class TypeService : ITypeService
    {
        private Dictionary<Type, int> _typeCounts = new Dictionary<Type, int>();

        public void Add(Type type)
        {
            if (_typeCounts.TryGetValue(type, out int value))
                _typeCounts[type] = value++;
            else
                _typeCounts.Add(type, 1);
        }

        public int GetCount(Type type) => _typeCounts[type];

        public TypesData GetTypesData()
        {
            Dictionary<Type, bool> isWritesFullNames = new Dictionary<Type, bool>(_typeCounts.Count);
            Dictionary<string, int> namespaceCounts = new Dictionary<string, int>();
            foreach (var item in _typeCounts)
            {
                string @namespace = item.Key.Namespace;
                if (namespaceCounts.TryGetValue(@namespace, out int value))
                    namespaceCounts[@namespace] = value + item.Value;
                else
                    namespaceCounts.Add(@namespace, item.Value);
            }

            string[] namespaces = namespaceCounts.OrderBy(nc => nc.Value).Select(nc => nc.Key).ToArray();

            foreach (var item in _typeCounts)
            {
                Type type = item.Key;
                bool isWritingFullName = false;
                if (type.Namespace != null)
                {
                    isWritingFullName = TypeHelper.Exists(type.Name);
                    if (!isWritingFullName)
                    {
                        foreach (var @namespace in namespaces)
                        {
                            if (type.Namespace != @namespace)
                            {
                                isWritingFullName = TypeHelper.Exists($"{@namespace}.{type.Name}");
                                if (isWritingFullName)
                                    break;
                            }
                        }
                    }
                }

                isWritesFullNames.Add(type, isWritingFullName);
            }

            return new TypesData(namespaces, isWritesFullNames);
        }
    }
}
