using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wild.Cson.Serialization.Deserializators.Utils
{
    public interface IFieldInfoDictionary 
    {
        FieldInfo GetAndRemove(string name); 
    }

    public interface ITypeMemberService
    {
        IFieldInfoDictionary GetSerializableMemberDictionary(Type type);
    }

    public sealed class TypeMemberService : ITypeMemberService
    {
        private class FieldInfoDictionary : IFieldInfoDictionary
        {
            private class FieldInfoExistsPair
            {
                public readonly FieldInfo FieldInfo;
                public bool IsExists = true;

                public FieldInfoExistsPair(FieldInfo fieldInfo) => FieldInfo = fieldInfo;
            }

            private readonly Dictionary<string, FieldInfoExistsPair> _nameFieldInfoExistsPairs = new Dictionary<string, FieldInfoExistsPair>();

            public FieldInfoDictionary(IEnumerable<FieldInfo> fieldInfos)
            {
                foreach (var fieldInfo in fieldInfos)
                    _nameFieldInfoExistsPairs.Add(fieldInfo.Name, new FieldInfoExistsPair(fieldInfo));
            }
            public FieldInfo GetAndRemove(string name)
            {
                _nameFieldInfoExistsPairs.TryGetValue(name, out FieldInfoExistsPair fieldInfoExistsPair);
                if (fieldInfoExistsPair == null || !fieldInfoExistsPair.IsExists)
                    throw new KeyNotFoundException($"FieldInfo {name} is not found!");
                fieldInfoExistsPair.IsExists = false;
                return fieldInfoExistsPair.FieldInfo;
            }

            public void Reset()
            {
                foreach (var item in _nameFieldInfoExistsPairs)
                    item.Value.IsExists = true;
            }
        }

        private Dictionary<Type, FieldInfoDictionary> _typeMemberDictionarys = new Dictionary<Type, FieldInfoDictionary>();

        public IFieldInfoDictionary GetSerializableMemberDictionary(Type type)
        {
            if (_typeMemberDictionarys.TryGetValue(type, out FieldInfoDictionary fieldInfoDictionary))
            {
                fieldInfoDictionary.Reset();
                return fieldInfoDictionary;
            }
            var members = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => !f.IsInitOnly);
            fieldInfoDictionary = new FieldInfoDictionary(members);
            _typeMemberDictionarys.Add(type, fieldInfoDictionary);
            return fieldInfoDictionary;
        }
    }
}
